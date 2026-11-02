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

public partial class CDS_WebPage_RESEARCH_TB_PRODUCT_CLASS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
            BindGrid2();
        }
    }


    #region FUNCTION
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        //TextBox1
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND (CLASSNAMES LIKE '%{0}%'  OR PRODNAMES LIKE '%{0}%' ) ", TextBox1.Text);
        }
        else
        {
            QUERYS.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                            SELECT 
                            [CLASSNAMES] AS '類別'
                            ,[PRODNAMES] AS '產品'
                            ,[COSTS] AS '成本結構'
                            ,[VALIDMARKETS] AS '效期評估市場'
                            ,[VALIDPRODS] AS '效期評估生產'
                            ,[MINPRODS] AS '最小批量'
                            ,[DAILYPRODS] AS '日產量'
                            ,[KEYMATERIALS] AS '關鍵原料'
                            ,[KEYPRODS] AS '關鍵製程'
                            ,[VEGAN] AS '素別'
                            ,CONVERT(NVARCHAR,[CREATEDATES],111) AS CREATEDATES
                            ,CONVERT(NVARCHAR,[UPDATEDATES],111) AS UPDATEDATES
                            , [ID]
                            FROM [TKRESEARCH].[dbo].[TB_PRODUCT_CLASS]
                            WHERE 1=1
                            {0}
                            ORDER BY [CLASSNAMES],[PRODNAMES]
                             ", QUERYS.ToString());




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
        //SETEXCEL();

    }
    private void BindGrid2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        //TextBox1
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND (CLASSNAMES LIKE '%{0}%'  OR PRODNAMES LIKE '%{0}%' ) ", TextBox1.Text);
        }
        else
        {
            QUERYS.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                            SELECT 
                            [CLASSNAMES] AS '類別'
                            ,[PRODNAMES] AS '產品'
                            ,[COSTS] AS '成本結構'
                            ,[VALIDMARKETS] AS '效期評估市場'
                            ,[VALIDPRODS] AS '效期評估生產'
                            ,[MINPRODS] AS '最小批量'
                            ,[DAILYPRODS] AS '日產量'
                            ,[KEYMATERIALS] AS '關鍵原料'
                            ,[KEYPRODS] AS '關鍵製程'
                            ,[VEGAN] AS '素別'
                            ,CONVERT(NVARCHAR,[CREATEDATES],111) AS CREATEDATES
                            ,CONVERT(NVARCHAR,[UPDATEDATES],111) AS UPDATEDATES
                            , [ID]
                            FROM [TKRESEARCH].[dbo].[TB_PRODUCT_CLASS]
                            WHERE 1=1
                            {0}
                            ORDER BY [CLASSNAMES],[PRODNAMES]
                             ", QUERYS.ToString());





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
            Button btn2 = (Button)e.Row.FindControl("Button2");
            if (btn2 != null)
            {
                string cellValue2 = btn2.CommandArgument;
                dynamic param2 = new { ID = cellValue2 }.ToExpando();
            }
        }
    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
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
                GridViewRow row = Grid2.Rows[rowIndex];

                Label Label_ID = (Label)row.FindControl("Label_ID");

                TextBox txtNewField_類別 = (TextBox)row.FindControl("txtNewField_類別");
                TextBox txtNewField_產品 = (TextBox)row.FindControl("txtNewField_產品");
                TextBox txtNewField_成本結構 = (TextBox)row.FindControl("txtNewField_成本結構");
                TextBox txtNewField_效期評估市場 = (TextBox)row.FindControl("txtNewField_效期評估市場");
                TextBox txtNewField_效期評估生產 = (TextBox)row.FindControl("txtNewField_效期評估生產");
                TextBox txtNewField_最小批量 = (TextBox)row.FindControl("txtNewField_最小批量");
                TextBox txtNewField_日產量 = (TextBox)row.FindControl("txtNewField_日產量");
                TextBox txtNewField_關鍵原料 = (TextBox)row.FindControl("txtNewField_關鍵原料");
                TextBox txtNewField_關鍵製程 = (TextBox)row.FindControl("txtNewField_關鍵製程");
                TextBox txtNewField_素別 = (TextBox)row.FindControl("txtNewField_素別");

                string ID = Label_ID.Text;
                string CLASSNAMES = txtNewField_類別.Text;
                string PRODNAMES = txtNewField_產品.Text;
                string COSTS = txtNewField_成本結構.Text;
                string VALIDMARKETS = txtNewField_效期評估市場.Text;
                string VALIDPRODS = txtNewField_效期評估生產.Text;
                string MINPRODS = txtNewField_最小批量.Text;
                string DAILYPRODS = txtNewField_日產量.Text;
                string KEYMATERIALS = txtNewField_關鍵原料.Text;
                string KEYPRODS = txtNewField_關鍵製程.Text;
                string VEGAN = txtNewField_素別.Text;

                UPDATE_TB_PRODUCT_CLASS(
                          ID,
                          CLASSNAMES,
                          PRODNAMES,
                          COSTS,
                          VALIDMARKETS,
                          VALIDPRODS,
                          MINPRODS,
                          DAILYPRODS,
                          KEYMATERIALS,
                          KEYPRODS,
                          VEGAN
                          );
            }

            BindGrid();
            BindGrid2();
        }

    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }

    public void UPDATE_TB_PRODUCT_CLASS(
        string ID,
        string CLASSNAMES,
        string PRODNAMES,
        string COSTS,
        string VALIDMARKETS,
        string VALIDPRODS,
        string MINPRODS,
        string DAILYPRODS,
        string KEYMATERIALS,
        string KEYPRODS,
        string VEGAN
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @"                           
                           UPDATE [TKRESEARCH].[dbo].[TB_PRODUCT_CLASS]
                            SET 
                            [CLASSNAMES]=@CLASSNAMES
                            ,[PRODNAMES]=@PRODNAMES
                            ,[COSTS]=@COSTS
                            ,[VALIDMARKETS]=@VALIDMARKETS
                            ,[VALIDPRODS]=@VALIDPRODS
                            ,[MINPRODS]=@MINPRODS
                            ,[DAILYPRODS]=@DAILYPRODS
                            ,[KEYMATERIALS]=@KEYMATERIALS
                            ,[KEYPRODS]=@KEYPRODS
                            ,[UPDATEDATES]=@UPDATEDATES
                            ,[VEGAN]=@VEGAN
                            WHERE [ID]=@ID
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@CLASSNAMES", CLASSNAMES);
                    cmd.Parameters.AddWithValue("@PRODNAMES", PRODNAMES);
                    cmd.Parameters.AddWithValue("@COSTS", COSTS);
                    cmd.Parameters.AddWithValue("@VALIDMARKETS", VALIDMARKETS);
                    cmd.Parameters.AddWithValue("@VALIDPRODS", VALIDPRODS);
                    cmd.Parameters.AddWithValue("@MINPRODS", MINPRODS);
                    cmd.Parameters.AddWithValue("@DAILYPRODS", DAILYPRODS);
                    cmd.Parameters.AddWithValue("@KEYMATERIALS", KEYMATERIALS);
                    cmd.Parameters.AddWithValue("@KEYPRODS", KEYPRODS);
                    cmd.Parameters.AddWithValue("@VEGAN", VEGAN);
                    cmd.Parameters.AddWithValue("@UPDATEDATES", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));



                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected >= 1)
                    {
                        //MsgBox(NO + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch
        {
        }
        finally
        {
        }
    }

    public void ADD_TB_PRODUCT_CLASS(
        string CLASSNAMES,
        string PRODNAMES,
        string COSTS,
        string VALIDMARKETS,
        string VALIDPRODS,
        string MINPRODS,
        string DAILYPRODS,
        string KEYMATERIALS,
        string KEYPRODS,
        string VEGAN
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                            INSERT INTO [TKRESEARCH].[dbo].[TB_PRODUCT_CLASS]
                            (
                            [CLASSNAMES]
                            ,[PRODNAMES]
                            ,[COSTS]
                            ,[VALIDMARKETS]
                            ,[VALIDPRODS]
                            ,[MINPRODS]
                            ,[DAILYPRODS]
                            ,[KEYMATERIALS]
                            ,[KEYPRODS]
                            ,[VEGAN]
                            )
                            VALUES
                            (
                            @CLASSNAMES
                            ,@PRODNAMES
                            ,@COSTS
                            ,@VALIDMARKETS
                            ,@VALIDPRODS
                            ,@MINPRODS
                            ,@DAILYPRODS
                            ,@KEYMATERIALS
                            ,@KEYPRODS
                            ,@VEGAN
                            )
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@CLASSNAMES", CLASSNAMES);
                    cmd.Parameters.AddWithValue("@PRODNAMES", PRODNAMES);
                    cmd.Parameters.AddWithValue("@COSTS", COSTS);
                    cmd.Parameters.AddWithValue("@VALIDMARKETS", VALIDMARKETS);
                    cmd.Parameters.AddWithValue("@VALIDPRODS", VALIDPRODS);
                    cmd.Parameters.AddWithValue("@MINPRODS", MINPRODS);
                    cmd.Parameters.AddWithValue("@DAILYPRODS", DAILYPRODS);
                    cmd.Parameters.AddWithValue("@KEYMATERIALS", KEYMATERIALS);
                    cmd.Parameters.AddWithValue("@KEYPRODS", KEYPRODS);
                    cmd.Parameters.AddWithValue("@VEGAN", VEGAN);


                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected >= 1)
                    {
                        MsgBox(PRODNAMES + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch
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
        BindGrid();
        BindGrid2();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {

        string CLASSNAMES = NEW_類別.Text.Trim();
        string PRODNAMES = NEW_產品.Text.Trim();
        string COSTS = NEW_成本結構.Text.Trim();
        string VALIDMARKETS = NEW_效期評估市場.Text.Trim();
        string VALIDPRODS = NEW_效期評估生產.Text.Trim();
        string MINPRODS = NEW_最小批量.Text.Trim();
        string DAILYPRODS = NEW_日產量.Text.Trim();
        string KEYMATERIALS = NEW_關鍵原料.Text.Trim();
        string KEYPRODS = NEW_關鍵製程.Text.Trim();
        string VEGAN = NEW_素別.Text.Trim();

        if (!string.IsNullOrEmpty(CLASSNAMES) )
        {

            ADD_TB_PRODUCT_CLASS(
                CLASSNAMES,
                PRODNAMES,
                COSTS,
                VALIDMARKETS,
                VALIDPRODS,
                MINPRODS,
                DAILYPRODS,
                KEYMATERIALS,
                KEYPRODS,
                VEGAN
                );

            BindGrid();
            BindGrid2();
        }
        else
        {
            MsgBox("類別不可空白", this.Page, this);
        }


    }
    #endregion
}