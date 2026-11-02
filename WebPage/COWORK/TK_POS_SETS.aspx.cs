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

public partial class CDS_WebPage_COWORK_TK_POS_SETS : Ede.Uof.Utility.Page.BasePage
{
    string MB003 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string ACCOUNT = Current.Account;
        string NAME = Current.User.Name;

        if (!IsPostBack)
        {
            SETDATES();
            BindDropDownList();
            BindDropDownList2();
            BindDropDownList3();
            BindDropDownList4();
            BindDropDownList5();
            BindDropDownList6();
            BindDropDownList7();

            //商品特價折扣
            BindGrid();
            //商品類別特價設定作業
            BindGrid2();
            //商品價格區間特價設定作業
            BindGrid3();
            //組合品搭贈設定作業
            BindGrid4();
            //配對搭贈設定作業
            BindGrid5();
            //滿額折價設定作業
            BindGrid6();
            //付款方式特價設定作業
            BindGrid7();
        }
    }
    #region FUNCTION
    public void SETDATES()
    {
        TextBox1.Text = DateTime.Now.ToString("yyyy");
        TextBox3.Text = DateTime.Now.ToString("yyyy");
        TextBox5.Text = DateTime.Now.ToString("yyyy");
        TextBox7.Text = DateTime.Now.ToString("yyyy");
        TextBox9.Text = DateTime.Now.ToString("yyyy");
        TextBox11.Text = DateTime.Now.ToString("yyyy");
        TextBox13.Text = DateTime.Now.ToString("yyyy");
    

    }
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "STATUS";
            DropDownList1.DataValueField = "STATUS";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }
    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "STATUS";
            DropDownList2.DataValueField = "STATUS";
            DropDownList2.DataBind();

        }
        else
        {

        }



    }
    private void BindDropDownList3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList3.DataSource = dt;
            DropDownList3.DataTextField = "STATUS";
            DropDownList3.DataValueField = "STATUS";
            DropDownList3.DataBind();

        }
        else
        {

        }



    }
    private void BindDropDownList4()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList4.DataSource = dt;
            DropDownList4.DataTextField = "STATUS";
            DropDownList4.DataValueField = "STATUS";
            DropDownList4.DataBind();

        }
        else
        {

        }



    }
    private void BindDropDownList5()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList5.DataSource = dt;
            DropDownList5.DataTextField = "STATUS";
            DropDownList5.DataValueField = "STATUS";
            DropDownList5.DataBind();

        }
        else
        {

        }



    }
    private void BindDropDownList6()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList6.DataSource = dt;
            DropDownList6.DataTextField = "STATUS";
            DropDownList6.DataValueField = "STATUS";
            DropDownList6.DataBind();

        }
        else
        {

        }



    }
    private void BindDropDownList7()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList7.DataSource = dt;
            DropDownList7.DataTextField = "STATUS";
            DropDownList7.DataValueField = "STATUS";
            DropDownList7.DataBind();

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

        //日期
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND MB001 LIKE '{0}%' ", TextBox1.Text.Trim());
        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("未核單"))
            {
                QUERYS2.AppendFormat(@" AND MB008='N' ");
            }
            else if (DropDownList1.Text.Equals("已核單"))
            {
                QUERYS2.AppendFormat(@"  AND MB008='Y' ");
            }
        }
        //特價名稱
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS3.AppendFormat(@" AND (MB004 LIKE '%{0}%' OR MB003 LIKE '%{0}%') ", TextBox2.Text.Trim());
        }




        cmdTxt.AppendFormat(@" 
                            SELECT *
                            ,(
                                       SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(13) + CHAR(10) 
                                       FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                       WHERE MF004=MA001 AND MF003 = MB003
                                       FOR XML PATH('')) AS All_MF004
                            ,(
                                       SELECT  LTRIM(RTRIM(NI002))+ CHAR(13) + CHAR(10) 
                                       FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                       WHERE MG005=NI001 AND MG003 = MB003
                                       FOR XML PATH('')) AS All_NI002
                            ,(
                                        SELECT  LTRIM(RTRIM(MC004))+ CHAR(13) + CHAR(10) +LTRIM(RTRIM(MB002))+ CHAR(13) + CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MC005))+ CHAR(13) + CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MC006))+ CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMC,[TK].dbo.INVMB
                                        WHERE MC004=INVMB.MB001 AND MC003 = POSMB.MB003
                                        FOR XML PATH('')) AS All_MC004
                            ,(MB012+'~'+MB013) AS 'MB012MB013'

                            FROM [TK].dbo.POSMB
                            WHERE 1=1  
                            AND MB002 IN ('1')
                            {0}
                            {1}   
                            {2}    

                                ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString());



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
            //Grid1_Button1
            //Get the button that raised the event
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid1_Button1");
            //Get the row that contains this button
            GridViewRow gvr1 = (GridViewRow)Grid1_Button1.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue1 = Grid1_Button1.CommandArgument;
            DataRowView row1 = (DataRowView)e.Row.DataItem;
            Button lbtnName1 = (Button)e.Row.FindControl("Grid1_Button1");
            ExpandoObject param1 = new { ID = Cellvalue1 }.ToExpando();

        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid1_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "", this.Page, this);

            MB003 = e.CommandArgument.ToString();

            //有重疊日期的品號
            DataTable DT= CHECK_POSMB_POSMI_POSMO(MB003);
            if(DT!=null && DT.Rows.Count>=1)
            {
                StringBuilder MESSAGES=new StringBuilder();
                MESSAGES.AppendFormat(@"");
                foreach(DataRow DR in DT.Rows)
                {
                    MESSAGES.AppendFormat(@"有錯誤，不可送簽 \r\n");
                    MESSAGES.AppendFormat(@"代號 ={0} 的 品號 ={1} 有重疊日期",DR["MB2MB003"].ToString().Trim(), DR["MC004"].ToString().Trim());
                }


                MsgBox(MESSAGES.ToString() + "", this.Page, this);
            }
            else
            {
                ADDTB_WKF_EXTERNAL_TASK_POSSET("商品特價折扣", MB003);
            }
            
        }

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

        //日期
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            QUERYS.AppendFormat(@" AND MB001 LIKE '{0}%' ", TextBox3.Text.Trim());
        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList2.Text))
        {
            if (DropDownList2.Text.Equals("未核單"))
            {
                QUERYS2.AppendFormat(@" AND MB008='N' ");
            }
            else if (DropDownList2.Text.Equals("已核單"))
            {
                QUERYS2.AppendFormat(@"  AND MB008='Y' ");
            }
        }
        //特價名稱
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            QUERYS3.AppendFormat(@" AND (MB004 LIKE '%{0}%' OR MB003 LIKE '%{0}%') ", TextBox4.Text.Trim());
        }




        cmdTxt.AppendFormat(@" 
                           SELECT *
                            ,(
                                        SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                        WHERE MF004=MA001 AND MF003 = MB003
                                        FOR XML PATH('')) AS All_MF004
                            ,(
                                        SELECT  LTRIM(RTRIM(NI002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                        WHERE MG005=NI001 AND MG003 = MB003
                                        FOR XML PATH('')) AS All_NI002
                            ,(
                                        SELECT  LTRIM(RTRIM(MD004))+ CHAR(13) + CHAR(10) +LTRIM(RTRIM(MA003))+ CHAR(13) + CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MD005))+ CHAR(13) + CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MD006))+ CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMD,[TK].dbo.INVMA
                                        WHERE MD004=INVMA.MA002 AND INVMA.MA001='3' AND MD003 = POSMB.MB003
                                        FOR XML PATH('')) AS All_MC004
                            ,(MB012+'~'+MB013) AS 'MB012MB013'

                            FROM [TK].dbo.POSMB
                            WHERE 1=1  
                            AND MB002 IN ('2')
                            {0}
                            {1}   
                            {2}    

                                ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Grid1_Button1
            //Get the button that raised the event
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid2_Button1");
            //Get the row that contains this button
            GridViewRow gvr1 = (GridViewRow)Grid1_Button1.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue1 = Grid1_Button1.CommandArgument;
            DataRowView row1 = (DataRowView)e.Row.DataItem;
            Button lbtnName1 = (Button)e.Row.FindControl("Grid2_Button1");
            ExpandoObject param1 = new { ID = Cellvalue1 }.ToExpando();

        }
    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid2_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "", this.Page, this);

            string MB003 = e.CommandArgument.ToString();

            //有重疊日期的品號
            DataTable DT = CHECK_POSMB_POSMI_POSMO(MB003);
            if (DT != null && DT.Rows.Count >= 1)
            {
                StringBuilder MESSAGES = new StringBuilder();
                MESSAGES.AppendFormat(@"");
                foreach (DataRow DR in DT.Rows)
                {
                    MESSAGES.AppendFormat(@"有錯誤，不可送簽 \r\n");
                    MESSAGES.AppendFormat(@"代號 ={0} 的 品號 ={1} 有重疊日期", DR["MB2MB003"].ToString().Trim(), DR["MC004"].ToString().Trim());
                }


                MsgBox(MESSAGES.ToString() + "", this.Page, this);
            }
            else
            {
                ADDTB_WKF_EXTERNAL_TASK_POSSET2("商品類別特價設定作業", MB003);
            }
            
        }

    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    private void BindGrid3()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox5.Text))
        {
            QUERYS.AppendFormat(@" AND MB001 LIKE '{0}%' ", TextBox5.Text.Trim());
        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList3.Text))
        {
            if (DropDownList3.Text.Equals("未核單"))
            {
                QUERYS2.AppendFormat(@" AND MB008='N' ");
            }
            else if (DropDownList3.Text.Equals("已核單"))
            {
                QUERYS2.AppendFormat(@"  AND MB008='Y' ");
            }
        }
        //特價名稱
        if (!string.IsNullOrEmpty(TextBox6.Text))
        {
            QUERYS3.AppendFormat(@" AND (MB004 LIKE '%{0}%' OR MB003 LIKE '%{0}%') ", TextBox6.Text.Trim());
        }




        cmdTxt.AppendFormat(@" 
                           SELECT *
                            ,(
                                        SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                        WHERE MF004=MA001 AND MF003 = MB003
                                        FOR XML PATH('')) AS All_MF004
                            ,(
                                        SELECT  LTRIM(RTRIM(NI002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                        WHERE MG005=NI001 AND MG003 = MB003
                                        FOR XML PATH('')) AS All_NI002
                            ,(
                                        SELECT  LTRIM(RTRIM(CONVERT(INT,ME005)))+ '~' +LTRIM(RTRIM(CONVERT(INT,ME006)))+ CHAR(13) + CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,ME007))+ CHAR(13) + CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,ME008))+ CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSME
                                        WHERE  ME003 = POSMB.MB003
                                        FOR XML PATH('')) AS All_MC004
                            ,(MB012+'~'+MB013) AS 'MB012MB013'

                            FROM [TK].dbo.POSMB
                            WHERE 1=1      
                            AND MB002 IN ('3')
                            {0}
                            {1}   
                            {2}    

                                ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid3.DataSource = dt;
        Grid3.DataBind();
    }

    protected void grid_PageIndexChanging3(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Grid1_Button1
            //Get the button that raised the event
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid3_Button1");
            //Get the row that contains this button
            GridViewRow gvr1 = (GridViewRow)Grid1_Button1.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue1 = Grid1_Button1.CommandArgument;
            DataRowView row1 = (DataRowView)e.Row.DataItem;
            Button lbtnName1 = (Button)e.Row.FindControl("Grid3_Button1");
            ExpandoObject param1 = new { ID = Cellvalue1 }.ToExpando();

        }
    }

    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid3_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "", this.Page, this);

            string MB003 = e.CommandArgument.ToString();

            //有重疊日期的品號
            DataTable DT = CHECK_POSMB_POSMI_POSMO(MB003);
            if (DT != null && DT.Rows.Count >= 1)
            {
                StringBuilder MESSAGES = new StringBuilder();
                MESSAGES.AppendFormat(@"");
                foreach (DataRow DR in DT.Rows)
                {
                    MESSAGES.AppendFormat(@"有錯誤，不可送簽 \r\n");
                    MESSAGES.AppendFormat(@"代號 ={0} 的 品號 ={1} 有重疊日期", DR["MB2MB003"].ToString().Trim(), DR["MC004"].ToString().Trim());
                }


                MsgBox(MESSAGES.ToString() + "", this.Page, this);
            }
            else
            {
                ADDTB_WKF_EXTERNAL_TASK_POSSET3("商品價格區間特價設定作業", MB003);
            }

            
        }

    }


    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    private void BindGrid4()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox7.Text))
        {
            QUERYS.AppendFormat(@" AND MI001 LIKE '{0}%' ", TextBox7.Text.Trim());
        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList4.Text))
        {
            if (DropDownList4.Text.Equals("未核單"))
            {
                QUERYS2.AppendFormat(@" AND MI015='N' ");
            }
            else if (DropDownList4.Text.Equals("已核單"))
            {
                QUERYS2.AppendFormat(@"  AND MI015='Y' ");
            }
        }
        //特價名稱
        if (!string.IsNullOrEmpty(TextBox8.Text))
        {
            QUERYS3.AppendFormat(@" AND (MI003 LIKE '%{0}%' OR MI004 LIKE '%{0}%') ", TextBox8.Text.Trim());
        }




        cmdTxt.AppendFormat(@" 
                            SELECT *
                            ,(
                                        SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                        WHERE MF004=MA001 AND MF003 = MI003
                                        FOR XML PATH('')) AS All_MF004                                     
                            ,(
                                        SELECT  LTRIM(RTRIM(NI002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                        WHERE MG005=NI001 AND MG003 = MI003
                                        FOR XML PATH('')) AS All_NI002
                            ,(
                                        SELECT  LTRIM(RTRIM(MJ004))+ CHAR(13) + CHAR(10)+LTRIM(RTRIM(MB002))+ CHAR(13) + CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MI017))+ CHAR(13) + CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MI018))+ CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMJ,[TK].dbo.INVMB
                                        WHERE MJ004=MB001 AND  MJ003 = MI003
                                        FOR XML PATH('')) AS All_MC004
                            ,(MI005+'~'+MI006) AS 'MB012MB013'

                            FROM [TK].dbo.POSMI
                            WHERE 1=1                             
                            {0}
                            {1}   
                            {2}    

                                ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid4.DataSource = dt;
        Grid4.DataBind();
    }

    protected void grid_PageIndexChanging4(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Grid1_Button1
            //Get the button that raised the event
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid4_Button1");
            //Get the row that contains this button
            GridViewRow gvr1 = (GridViewRow)Grid1_Button1.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue1 = Grid1_Button1.CommandArgument;
            DataRowView row1 = (DataRowView)e.Row.DataItem;
            Button lbtnName1 = (Button)e.Row.FindControl("Grid4_Button1");
            ExpandoObject param1 = new { ID = Cellvalue1 }.ToExpando();

        }
    }

    protected void Grid4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid4_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "", this.Page, this);

            string MI003 = e.CommandArgument.ToString();

            //有重疊日期的品號
            DataTable DT = CHECK_POSMB_POSMI_POSMO(MI003);
            if (DT != null && DT.Rows.Count >= 1)
            {
                StringBuilder MESSAGES = new StringBuilder();
                MESSAGES.AppendFormat(@"");
                foreach (DataRow DR in DT.Rows)
                {
                    MESSAGES.AppendFormat(@"有錯誤，不可送簽 \r\n");
                    MESSAGES.AppendFormat(@"代號 ={0} 的 品號 ={1} 有重疊日期", DR["MB2MB003"].ToString().Trim(), DR["MC004"].ToString().Trim());
                }


                MsgBox(MESSAGES.ToString() + "", this.Page, this);
            }
            else
            {
                ADDTB_WKF_EXTERNAL_TASK_POSSET4("組合品搭贈設定作業", MI003);
            }
            
        }

    }


    public void OnBeforeExport4(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    private void BindGrid5()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox9.Text))
        {
            QUERYS.AppendFormat(@" AND MO001 LIKE '{0}%' ", TextBox9.Text.Trim());
        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList5.Text))
        {
            if (DropDownList5.Text.Equals("未核單"))
            {
                QUERYS2.AppendFormat(@" AND MO008='N' ");
            }
            else if (DropDownList5.Text.Equals("已核單"))
            {
                QUERYS2.AppendFormat(@"  AND MO008='Y' ");
            }
        }
        //特價名稱
        if (!string.IsNullOrEmpty(TextBox10.Text))
        {
            QUERYS3.AppendFormat(@" AND (MO003 LIKE '%{0}%' OR MO004 LIKE '%{0}%') ", TextBox10.Text.Trim());
        }




        cmdTxt.AppendFormat(@" 
                           SELECT *
                            ,(
                                        SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                        WHERE MF004=MA001 AND MF003 = MO003
                                        FOR XML PATH('')) AS All_MF004                                     
                            ,(
                                        SELECT  LTRIM(RTRIM(NI002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                        WHERE MG005=NI001 AND MG003 = MO003
                                        FOR XML PATH('')) AS All_NI002
                            ,(
                                        SELECT  LTRIM(RTRIM(MP005))+ CHAR(13) + CHAR(10)+LTRIM(RTRIM(MB002))+ CHAR(13) + CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MP006))+ CHAR(13) + CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MP007))+ CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMP,[TK].dbo.INVMB
                                        WHERE MP005=MB001 AND  MP003 = MO003
                                        FOR XML PATH('')) AS All_MC004
                            ,(MO011+'~'+MO012) AS 'MB012MB013'

                            FROM [TK].dbo.POSMO
                            WHERE 1=1                            
                            {0}
                            {1}   
                            {2}    

                                ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid5.DataSource = dt;
        Grid5.DataBind();
    }

    protected void grid_PageIndexChanging5(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid5_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Grid1_Button1
            //Get the button that raised the event
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid5_Button1");
            //Get the row that contains this button
            GridViewRow gvr1 = (GridViewRow)Grid1_Button1.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue1 = Grid1_Button1.CommandArgument;
            DataRowView row1 = (DataRowView)e.Row.DataItem;
            Button lbtnName1 = (Button)e.Row.FindControl("Grid5_Button1");
            ExpandoObject param1 = new { ID = Cellvalue1 }.ToExpando();

        }
    }

    protected void Grid5_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid5_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "", this.Page, this);

            string MO003 = e.CommandArgument.ToString();

            //有重疊日期的品號
            DataTable DT = CHECK_POSMB_POSMI_POSMO(MO003);
            if (DT != null && DT.Rows.Count >= 1)
            {
                StringBuilder MESSAGES = new StringBuilder();
                MESSAGES.AppendFormat(@"");
                foreach (DataRow DR in DT.Rows)
                {
                    MESSAGES.AppendFormat(@"有錯誤，不可送簽 \r\n");
                    MESSAGES.AppendFormat(@"代號 ={0} 的 品號 ={1} 有重疊日期", DR["MB2MB003"].ToString().Trim(), DR["MC004"].ToString().Trim());
                }


                MsgBox(MESSAGES.ToString() + "", this.Page, this);
            }
            else
            {
                ADDTB_WKF_EXTERNAL_TASK_POSSET5("配對搭贈設定作業", MO003);
            }

           
        }

    }


    public void OnBeforeExport5(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    private void BindGrid6()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox11.Text))
        {
            QUERYS.AppendFormat(@" AND MM001 LIKE '{0}%' ", TextBox11.Text.Trim());
        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList6.Text))
        {
            if (DropDownList6.Text.Equals("未核單"))
            {
                QUERYS2.AppendFormat(@" AND MM015='N' ");
            }
            else if (DropDownList6.Text.Equals("已核單"))
            {
                QUERYS2.AppendFormat(@"  AND MM015='Y' ");
            }
        }
        //特價名稱
        if (!string.IsNullOrEmpty(TextBox12.Text))
        {
            QUERYS3.AppendFormat(@" AND (MM003 LIKE '%{0}%' OR MM004 LIKE '%{0}%') ", TextBox12.Text.Trim());
        }




        cmdTxt.AppendFormat(@" 
                            SELECT *
                            ,(
                                        SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                        WHERE MF004=MA001 AND MF003 = MM003
                                        FOR XML PATH('')) AS All_MF004                                     
                            ,(
                                        SELECT  LTRIM(RTRIM(NI002))+ CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                        WHERE MG005=NI001 AND MG003 = MM003
                                        FOR XML PATH('')) AS All_NI002
                            ,(
                                        SELECT  LTRIM(RTRIM(CONVERT(INT,MN005)))+' 以上'+ CHAR(13) + CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MN008))+ CHAR(13) + CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MN009))+ CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
                                        FROM [TK].dbo.POSMN
                                        WHERE  MN003 = MM003
                                        FOR XML PATH('')) AS All_MC004
                            ,(MM005+'~'+MM005) AS 'MB012MB013'

                            FROM [TK].dbo.POSMM
                            WHERE 1=1  
                            {0}
                            {1}   
                            {2}    

                                ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid6.DataSource = dt;
        Grid6.DataBind();
    }

    protected void grid_PageIndexChanging6(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid6_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Grid1_Button1
            //Get the button that raised the event
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid6_Button1");
            //Get the row that contains this button
            GridViewRow gvr1 = (GridViewRow)Grid1_Button1.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue1 = Grid1_Button1.CommandArgument;
            DataRowView row1 = (DataRowView)e.Row.DataItem;
            Button lbtnName1 = (Button)e.Row.FindControl("Grid6_Button1");
            ExpandoObject param1 = new { ID = Cellvalue1 }.ToExpando();

        }
    }

    protected void Grid6_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid6_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "", this.Page, this);

            string MM003 = e.CommandArgument.ToString();

            ADDTB_WKF_EXTERNAL_TASK_POSSET6("滿額折價設定作業", MM003);
        }

    }


    public void OnBeforeExport6(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    private void BindGrid7()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox13.Text))
        {
            QUERYS.AppendFormat(@" AND MB001 LIKE '{0}%' ", TextBox13.Text.Trim());
        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList7.Text))
        {
            if (DropDownList7.Text.Equals("未核單"))
            {
                QUERYS2.AppendFormat(@" AND MB008='N' ");
            }
            else if (DropDownList7.Text.Equals("已核單"))
            {
                QUERYS2.AppendFormat(@"  AND MB008='Y' ");
            }
        }
        //特價名稱
        if (!string.IsNullOrEmpty(TextBox14.Text))
        {
            QUERYS3.AppendFormat(@" AND (MB003 LIKE '%{0}%' OR MB004 LIKE '%{0}%') ", TextBox14.Text.Trim());
        }




        cmdTxt.AppendFormat(@" 
                            SELECT *
                            ,(
                                       SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(13) + CHAR(10) 
                                       FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                       WHERE MF004=MA001 AND MF003 = MB003
                                       FOR XML PATH('')) AS All_MF004
                            ,(
                                       SELECT  LTRIM(RTRIM(NI002))+ CHAR(13) + CHAR(10) 
                                       FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                       WHERE MG005=NI001 AND MG003 = MB003
                                       FOR XML PATH('')) AS All_NI002
                            ,(
                                        SELECT  LTRIM(RTRIM(MB027))+ CHAR(13) + CHAR(10) +LTRIM(RTRIM(MT003))+ CHAR(13) + CHAR(10)
                                        FROM [TK].dbo.POSMT,[TK].dbo.POSMB MB
                                        WHERE MT002=MB.MB027 AND MB.MB003 = POSMB.MB003
                                        FOR XML PATH('')) AS All_MC004
                             ,(MB012+'~'+MB013) AS 'MB012MB013'

                            FROM [TK].dbo.POSMB
                            WHERE 1=1  
                            AND MB002 IN ('8')
                            {0}
                            {1}   
                            {2}    

                                ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid7.DataSource = dt;
        Grid7.DataBind();
    }

    protected void grid_PageIndexChanging7(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid7_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Grid1_Button1
            //Get the button that raised the event
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid7_Button1");
            //Get the row that contains this button
            GridViewRow gvr1 = (GridViewRow)Grid1_Button1.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue1 = Grid1_Button1.CommandArgument;
            DataRowView row1 = (DataRowView)e.Row.DataItem;
            Button lbtnName1 = (Button)e.Row.FindControl("Grid7_Button1");
            ExpandoObject param1 = new { ID = Cellvalue1 }.ToExpando();

        }
    }

    protected void Grid7_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid7_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "", this.Page, this);

            string MB003 = e.CommandArgument.ToString();

            ADDTB_WKF_EXTERNAL_TASK_POSSET7("付款方式特價設定作業", MB003);
        }

    }


    public void OnBeforeExport7(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    //商品特價折扣作業
    public void ADDTB_WKF_EXTERNAL_TASK_POSSET(string KINDS,string MB003)
    {
        DataTable DT = SEARCH_POSMB(MB003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["CREATOR"].ToString());

        string account = DT.Rows[0]["CREATOR"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["MB003"].ToString().Trim() ;

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string ID = SEARCHFORM_VERSION_ID("POS,商品活動設定");

        if (!string.IsNullOrEmpty(ID))
        {
            Form.SetAttribute("formVersionId", ID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //FIELD001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MF004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_NI002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD007	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD007");
        FieldItem.SetAttribute("fieldValue", KINDS);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB012MB013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //用ADDTACK，直接啟動起單
        ADDTACK(Form);

    }

    //商品類別特價設定作業
    public void ADDTB_WKF_EXTERNAL_TASK_POSSET2(string KINDS, string MB003)
    {
        DataTable DT = SEARCH_POSMB2(MB003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["CREATOR"].ToString());

        string account = DT.Rows[0]["CREATOR"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["MB003"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string ID = SEARCHFORM_VERSION_ID("POS,商品活動設定");

        if (!string.IsNullOrEmpty(ID))
        {
            Form.SetAttribute("formVersionId", ID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //FIELD001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MF004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_NI002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD007	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD007");
        FieldItem.SetAttribute("fieldValue", KINDS);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB012MB013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //用ADDTACK，直接啟動起單
        ADDTACK(Form);

    }
    //商品價格區間特價設定作業
    public void ADDTB_WKF_EXTERNAL_TASK_POSSET3(string KINDS, string MB003)
    {
        DataTable DT = SEARCH_POSMB3(MB003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["CREATOR"].ToString());

        string account = DT.Rows[0]["CREATOR"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["MB003"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string ID = SEARCHFORM_VERSION_ID("POS,商品活動設定");

        if (!string.IsNullOrEmpty(ID))
        {
            Form.SetAttribute("formVersionId", ID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //FIELD001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MF004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_NI002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD007	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD007");
        FieldItem.SetAttribute("fieldValue", KINDS);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB012MB013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //用ADDTACK，直接啟動起單
        ADDTACK(Form);

    }
    //組合品搭贈設定作業
    public void ADDTB_WKF_EXTERNAL_TASK_POSSET4(string KINDS, string MI003)
    {
        DataTable DT = SEARCH_POSMI(MI003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["CREATOR"].ToString());

        string account = DT.Rows[0]["CREATOR"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["MI003"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string ID = SEARCHFORM_VERSION_ID("POS,商品活動設定");

        if (!string.IsNullOrEmpty(ID))
        {
            Form.SetAttribute("formVersionId", ID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //FIELD001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MI001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MI003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MI004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MF004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_NI002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD007	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD007");
        FieldItem.SetAttribute("fieldValue", KINDS);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB012MB013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //用ADDTACK，直接啟動起單
        ADDTACK(Form);

    }
    //配對搭贈設定作業
    public void ADDTB_WKF_EXTERNAL_TASK_POSSET5(string KINDS, string MO003)
    {
        DataTable DT = SEARCH_POSMO(MO003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["CREATOR"].ToString());

        string account = DT.Rows[0]["CREATOR"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["MO003"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string ID = SEARCHFORM_VERSION_ID("POS,商品活動設定");

        if (!string.IsNullOrEmpty(ID))
        {
            Form.SetAttribute("formVersionId", ID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //FIELD001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MO001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MO003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MO004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MF004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_NI002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD007	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD007");
        FieldItem.SetAttribute("fieldValue", KINDS);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB012MB013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //用ADDTACK，直接啟動起單
        ADDTACK(Form);

    }
    //滿額折價設定作業
    public void ADDTB_WKF_EXTERNAL_TASK_POSSET6(string KINDS, string MM003)
    {
        DataTable DT = SEARCH_POSMM(MM003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["CREATOR"].ToString());

        string account = DT.Rows[0]["CREATOR"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["MM003"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string ID = SEARCHFORM_VERSION_ID("POS,商品活動設定");

        if (!string.IsNullOrEmpty(ID))
        {
            Form.SetAttribute("formVersionId", ID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //FIELD001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MM001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MM003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MM004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MF004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_NI002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD007	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD007");
        FieldItem.SetAttribute("fieldValue", KINDS);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB012MB013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //用ADDTACK，直接啟動起單
        ADDTACK(Form);

    }
    //付款方式特價設定作業
    public void ADDTB_WKF_EXTERNAL_TASK_POSSET7(string KINDS, string MB003)
    {
        DataTable DT = SEARCH_POSMB4(MB003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["CREATOR"].ToString());

        string account = DT.Rows[0]["CREATOR"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["MB003"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string ID = SEARCHFORM_VERSION_ID("POS,商品活動設定");

        if (!string.IsNullOrEmpty(ID))
        {
            Form.SetAttribute("formVersionId", ID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //FIELD001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_MF004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["All_NI002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD007	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD007");
        FieldItem.SetAttribute("fieldValue", KINDS);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //FIELD008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "FIELD008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MB012MB013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //用ADDTACK，直接啟動起單
        ADDTACK(Form);

    }


    public string SEARCHFORM_VERSION_ID(string FORM_NAME)
    {
        try
        {

            string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();



            cmdTxt.AppendFormat(@" 
                                 SELECT TOP 1 RTRIM(LTRIM(TB_WKF_FORM_VERSION.FORM_VERSION_ID)) FORM_VERSION_ID,TB_WKF_FORM_VERSION.FORM_ID,TB_WKF_FORM_VERSION.VERSION,TB_WKF_FORM_VERSION.ISSUE_CTL
                                    ,TB_WKF_FORM.FORM_NAME
                                    FROM [UOF].dbo.TB_WKF_FORM_VERSION,[UOF].dbo.TB_WKF_FORM
                                    WHERE 1=1
                                    AND TB_WKF_FORM_VERSION.FORM_ID=TB_WKF_FORM.FORM_ID
                                    AND TB_WKF_FORM_VERSION.ISSUE_CTL=1
                                    AND FORM_NAME='{0}'
                                    ORDER BY TB_WKF_FORM_VERSION.FORM_ID,TB_WKF_FORM_VERSION.VERSION DESC

                                   ", FORM_NAME);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt.Rows.Count >= 1)
            {
                return dt.Rows[0]["FORM_VERSION_ID"].ToString();
            }
            else
            {
                return "";
            }

        }
        catch
        {
            return "";
        }
        finally
        {

        }
    }

    public DataTable SEARCH_POSMB(string MB003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                   SELECT TEMP.*
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'

                                    FROM 
                                    (
                                    SELECT
                                    POSMB.[CREATOR]
                                    ,POSMB.[MB001]
                                    ,POSMB.[MB002]
                                    ,POSMB.[MB003]
                                    ,POSMB.[MB004]
                                    ,POSMB.[MB005]
                                    ,POSMB.[MB006]
                                    ,POSMB.[MB007]
                                    ,POSMB.[MB008]
                                    ,POSMB.[MB009]
                                    ,POSMB.[MB010]
                                    ,POSMB.[MB011]
                                    ,POSMB.[MB012]
                                    ,POSMB.[MB013]
                                    ,POSMB.[MB014]
                                    ,POSMB.[MB015]
                                    ,POSMB.[MB016]
                                    ,POSMB.[MB017]
                                    ,POSMB.[MB018]
                                    ,POSMB.[MB019]
                                    ,POSMB.[MB020]
                                    ,POSMB.[MB021]
                                    ,POSMB.[MB022]
                                    ,POSMB.[MB023]
                                    ,POSMB.[MB024]
                                    ,POSMB.[MB025]
                                    ,POSMB.[MB026]
                                    ,POSMB.[MB027]
                                    ,POSMB.[MB028]
                                    ,POSMB.[MB029]
                                    ,POSMB.[MB030]
                                    ,POSMB.[MB031]
                                    ,POSMB.[MB032]
                                    ,POSMB.[MB033]
                                    ,POSMB.[MB034]
                                    ,POSMB.[MB035]
                                    ,POSMB.[MB036]
                                    ,POSMB.[MB037]
                                    ,POSMB.[MB038]
                                    ,POSMB.[MB039]
                                    ,POSMB.[MB040]
                                    ,POSMB.[MB041]
                                    ,POSMB.[MB042]
                                    ,POSMB.[MB043]
                                    ,POSMB.[MB044]
                                    ,POSMB.[MB045]
                                    ,POSMB.[MB046]
                                    ,POSMB.[MB200]
                                    ,POSMB.[UDF01]
                                    ,POSMB.[UDF02]
                                    ,POSMB.[UDF03]
                                    ,POSMB.[UDF04]
                                    ,POSMB.[UDF05]
                                    ,POSMB.[UDF06]
                                    ,POSMB.[UDF07]
                                    ,POSMB.[UDF08]
                                    ,POSMB.[UDF09]
                                    ,POSMB.[UDF10]
                                    ,(
                                    SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(10) 
                                    FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                    WHERE MF004=MA001 AND MF003 = MB003
                                    FOR XML PATH('')) AS All_MF004
                                    ,(
                                    SELECT  LTRIM(RTRIM(NI002)) + CHAR(10)
                                    FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                    WHERE MG005=NI001 AND MG003 = MB003
                                    FOR XML PATH('')) AS All_NI002
                                    ,(
                                    SELECT  LTRIM(RTRIM(MC004))+LTRIM(RTRIM(MB002))+'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MC005))+' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MC006))+ CHAR(10)
                                    FROM [TK].dbo.POSMC,[TK].dbo.INVMB
                                    WHERE MC004=INVMB.MB001 AND MC003 = POSMB.MB003
                                    FOR XML PATH('')) AS All_MC004
                                    ,(MB012+'~'+MB013) AS 'MB012MB013'
                                     ,[TB_EB_USER].USER_GUID,NAME
   

                                    FROM [TK].dbo.POSMB
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= POSMB.CREATOR COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE 1=1    
                                    AND MB003='{0}'
                                    ) AS TEMP
                              
                                    ", MB003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }

    public DataTable SEARCH_POSMB2(string MB003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                   SELECT TEMP.*
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'

                                    FROM 
                                    (
                                    SELECT
                                    POSMB.[CREATOR]
                                    ,POSMB.[MB001]
                                    ,POSMB.[MB002]
                                    ,POSMB.[MB003]
                                    ,POSMB.[MB004]
                                    ,POSMB.[MB005]
                                    ,POSMB.[MB006]
                                    ,POSMB.[MB007]
                                    ,POSMB.[MB008]
                                    ,POSMB.[MB009]
                                    ,POSMB.[MB010]
                                    ,POSMB.[MB011]
                                    ,POSMB.[MB012]
                                    ,POSMB.[MB013]
                                    ,POSMB.[MB014]
                                    ,POSMB.[MB015]
                                    ,POSMB.[MB016]
                                    ,POSMB.[MB017]
                                    ,POSMB.[MB018]
                                    ,POSMB.[MB019]
                                    ,POSMB.[MB020]
                                    ,POSMB.[MB021]
                                    ,POSMB.[MB022]
                                    ,POSMB.[MB023]
                                    ,POSMB.[MB024]
                                    ,POSMB.[MB025]
                                    ,POSMB.[MB026]
                                    ,POSMB.[MB027]
                                    ,POSMB.[MB028]
                                    ,POSMB.[MB029]
                                    ,POSMB.[MB030]
                                    ,POSMB.[MB031]
                                    ,POSMB.[MB032]
                                    ,POSMB.[MB033]
                                    ,POSMB.[MB034]
                                    ,POSMB.[MB035]
                                    ,POSMB.[MB036]
                                    ,POSMB.[MB037]
                                    ,POSMB.[MB038]
                                    ,POSMB.[MB039]
                                    ,POSMB.[MB040]
                                    ,POSMB.[MB041]
                                    ,POSMB.[MB042]
                                    ,POSMB.[MB043]
                                    ,POSMB.[MB044]
                                    ,POSMB.[MB045]
                                    ,POSMB.[MB046]
                                    ,POSMB.[MB200]
                                    ,POSMB.[UDF01]
                                    ,POSMB.[UDF02]
                                    ,POSMB.[UDF03]
                                    ,POSMB.[UDF04]
                                    ,POSMB.[UDF05]
                                    ,POSMB.[UDF06]
                                    ,POSMB.[UDF07]
                                    ,POSMB.[UDF08]
                                    ,POSMB.[UDF09]
                                    ,POSMB.[UDF10]
                                    ,(
                                    SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(10) 
                                    FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                    WHERE MF004=MA001 AND MF003 = MB003
                                    FOR XML PATH('')) AS All_MF004
                                    ,(
                                    SELECT  LTRIM(RTRIM(NI002)) + CHAR(10)
                                    FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                    WHERE MG005=NI001 AND MG003 = MB003
                                    FOR XML PATH('')) AS All_NI002
                                    ,(
                                    SELECT  LTRIM(RTRIM(MC004))+LTRIM(RTRIM(MB002))+'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MC005))+' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MC006))+ CHAR(10)
                                    FROM [TK].dbo.POSMC,[TK].dbo.INVMB
                                    WHERE MC004=INVMB.MB001 AND MC003 = POSMB.MB003
                                    FOR XML PATH('')) AS All_MC004
                                    ,(MB012+'~'+MB013) AS 'MB012MB013'
                                     ,[TB_EB_USER].USER_GUID,NAME


                                    FROM [TK].dbo.POSMB
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= POSMB.CREATOR COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE 1=1    
                                    AND MB003='{0}'
                                    ) AS TEMP
                              
                                    ", MB003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }
    public DataTable SEARCH_POSMB3(string MB003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                   SELECT TEMP.*
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'

                                    FROM 
                                    (
                                    SELECT
                                    POSMB.[CREATOR]
                                    ,POSMB.[MB001]
                                    ,POSMB.[MB002]
                                    ,POSMB.[MB003]
                                    ,POSMB.[MB004]
                                    ,POSMB.[MB005]
                                    ,POSMB.[MB006]
                                    ,POSMB.[MB007]
                                    ,POSMB.[MB008]
                                    ,POSMB.[MB009]
                                    ,POSMB.[MB010]
                                    ,POSMB.[MB011]
                                    ,POSMB.[MB012]
                                    ,POSMB.[MB013]
                                    ,POSMB.[MB014]
                                    ,POSMB.[MB015]
                                    ,POSMB.[MB016]
                                    ,POSMB.[MB017]
                                    ,POSMB.[MB018]
                                    ,POSMB.[MB019]
                                    ,POSMB.[MB020]
                                    ,POSMB.[MB021]
                                    ,POSMB.[MB022]
                                    ,POSMB.[MB023]
                                    ,POSMB.[MB024]
                                    ,POSMB.[MB025]
                                    ,POSMB.[MB026]
                                    ,POSMB.[MB027]
                                    ,POSMB.[MB028]
                                    ,POSMB.[MB029]
                                    ,POSMB.[MB030]
                                    ,POSMB.[MB031]
                                    ,POSMB.[MB032]
                                    ,POSMB.[MB033]
                                    ,POSMB.[MB034]
                                    ,POSMB.[MB035]
                                    ,POSMB.[MB036]
                                    ,POSMB.[MB037]
                                    ,POSMB.[MB038]
                                    ,POSMB.[MB039]
                                    ,POSMB.[MB040]
                                    ,POSMB.[MB041]
                                    ,POSMB.[MB042]
                                    ,POSMB.[MB043]
                                    ,POSMB.[MB044]
                                    ,POSMB.[MB045]
                                    ,POSMB.[MB046]
                                    ,POSMB.[MB200]
                                    ,POSMB.[UDF01]
                                    ,POSMB.[UDF02]
                                    ,POSMB.[UDF03]
                                    ,POSMB.[UDF04]
                                    ,POSMB.[UDF05]
                                    ,POSMB.[UDF06]
                                    ,POSMB.[UDF07]
                                    ,POSMB.[UDF08]
                                    ,POSMB.[UDF09]
                                    ,POSMB.[UDF10]
                                    ,(
                                    SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(10) 
                                    FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                    WHERE MF004=MA001 AND MF003 = MB003
                                    FOR XML PATH('')) AS All_MF004
                                    ,(
                                    SELECT  LTRIM(RTRIM(NI002)) + CHAR(10)
                                    FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                    WHERE MG005=NI001 AND MG003 = MB003
                                    FOR XML PATH('')) AS All_NI002
                                    ,(
                                    SELECT  LTRIM(RTRIM(MC004))+LTRIM(RTRIM(MB002))+'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MC005))+' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MC006))+ CHAR(10)
                                    FROM [TK].dbo.POSMC,[TK].dbo.INVMB
                                    WHERE MC004=INVMB.MB001 AND MC003 = POSMB.MB003
                                    FOR XML PATH('')) AS All_MC004
                                    ,(MB012+'~'+MB013) AS 'MB012MB013'
                                     ,[TB_EB_USER].USER_GUID,NAME
               

                                    FROM [TK].dbo.POSMB
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= POSMB.CREATOR COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE 1=1    
                                    AND MB003='{0}'
                                    ) AS TEMP
                              
                                    ", MB003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }

    public DataTable SEARCH_POSMB4(string MB003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                   SELECT TEMP.*
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'

                                    FROM 
                                    (
                                    SELECT
                                    POSMB.[CREATOR]
                                    ,POSMB.[MB001]
                                    ,POSMB.[MB002]
                                    ,POSMB.[MB003]
                                    ,POSMB.[MB004]
                                    ,POSMB.[MB005]
                                    ,POSMB.[MB006]
                                    ,POSMB.[MB007]
                                    ,POSMB.[MB008]
                                    ,POSMB.[MB009]
                                    ,POSMB.[MB010]
                                    ,POSMB.[MB011]
                                    ,POSMB.[MB012]
                                    ,POSMB.[MB013]
                                    ,POSMB.[MB014]
                                    ,POSMB.[MB015]
                                    ,POSMB.[MB016]
                                    ,POSMB.[MB017]
                                    ,POSMB.[MB018]
                                    ,POSMB.[MB019]
                                    ,POSMB.[MB020]
                                    ,POSMB.[MB021]
                                    ,POSMB.[MB022]
                                    ,POSMB.[MB023]
                                    ,POSMB.[MB024]
                                    ,POSMB.[MB025]
                                    ,POSMB.[MB026]
                                    ,POSMB.[MB027]
                                    ,POSMB.[MB028]
                                    ,POSMB.[MB029]
                                    ,POSMB.[MB030]
                                    ,POSMB.[MB031]
                                    ,POSMB.[MB032]
                                    ,POSMB.[MB033]
                                    ,POSMB.[MB034]
                                    ,POSMB.[MB035]
                                    ,POSMB.[MB036]
                                    ,POSMB.[MB037]
                                    ,POSMB.[MB038]
                                    ,POSMB.[MB039]
                                    ,POSMB.[MB040]
                                    ,POSMB.[MB041]
                                    ,POSMB.[MB042]
                                    ,POSMB.[MB043]
                                    ,POSMB.[MB044]
                                    ,POSMB.[MB045]
                                    ,POSMB.[MB046]
                                    ,POSMB.[MB200]
                                    ,POSMB.[UDF01]
                                    ,POSMB.[UDF02]
                                    ,POSMB.[UDF03]
                                    ,POSMB.[UDF04]
                                    ,POSMB.[UDF05]
                                    ,POSMB.[UDF06]
                                    ,POSMB.[UDF07]
                                    ,POSMB.[UDF08]
                                    ,POSMB.[UDF09]
                                    ,POSMB.[UDF10]
                                    ,(
                                    SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(10) 
                                    FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                    WHERE MF004=MA001 AND MF003 = MB003
                                    FOR XML PATH('')) AS All_MF004
                                    ,(
                                    SELECT  LTRIM(RTRIM(NI002)) + CHAR(10)
                                    FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                    WHERE MG005=NI001 AND MG003 = MB003
                                    FOR XML PATH('')) AS All_NI002
                                    ,(
                                    SELECT  LTRIM(RTRIM(MB027))+ CHAR(10) +LTRIM(RTRIM(MT003))+ CHAR(10)
                                    FROM [TK].dbo.POSMT,[TK].dbo.POSMB MB
                                    WHERE MT002=MB.MB027 AND MB.MB003 = POSMB.MB003
                                    FOR XML PATH('')) AS All_MC004
                                    ,(MB012+'~'+MB013) AS 'MB012MB013'
                                     ,[TB_EB_USER].USER_GUID,NAME
               

                                    FROM [TK].dbo.POSMB
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= POSMB.CREATOR COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE 1=1    
                                    AND MB003='{0}'
                                    ) AS TEMP
                              
                                    ", MB003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }

    public DataTable SEARCH_POSMI(string MI003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                  SELECT TEMP.*
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'

                                    FROM 
                                    (
                                    SELECT
                                    POSMI.[CREATOR]
                                    ,POSMI.[MI001]
                                    ,POSMI.[MI002]
                                    ,POSMI.[MI003]
                                    ,POSMI.[MI004]
                                    ,POSMI.[MI005]
                                    ,POSMI.[MI006]
                                    ,POSMI.[MI007]
                                    ,POSMI.[MI008]
                                    ,POSMI.[MI009]
                                    ,POSMI.[MI010]
                                    ,POSMI.[MI011]
                                    ,POSMI.[MI012]
                                    ,POSMI.[MI013]
                                    ,POSMI.[MI014]
                                    ,POSMI.[MI015]
                                    ,POSMI.[MI016]
                                    ,POSMI.[MI017]
                                    ,POSMI.[MI018]
                                    ,POSMI.[MI019]
                                    ,POSMI.[MI020]
                                    ,POSMI.[MI021]
                                    ,POSMI.[MI022]
                                    ,POSMI.[MI023]
                                    ,POSMI.[MI024]
                                    ,POSMI.[MI025]
                                    ,POSMI.[MI026]
                                    ,POSMI.[MI027]
                                    ,POSMI.[MI028]
                                    ,POSMI.[MI029]
                                    ,POSMI.[MI030]
                                    ,POSMI.[MI031]
                                    ,POSMI.[MI032]
                                    ,POSMI.[MI033]
                                    ,POSMI.[MI034]
                                    ,POSMI.[MI035]
                                    ,POSMI.[MI036]
                                    ,POSMI.[MI037]
                                    ,POSMI.[MI038]
                                    ,POSMI.[MI039]
                                    ,POSMI.[MI040]
                                    ,POSMI.[MI041]
                                    ,POSMI.[MI042]
                                    ,POSMI.[MI043]
                                    ,POSMI.[MI200]
                                    ,POSMI.[UDF01]
                                    ,POSMI.[UDF02]
                                    ,POSMI.[UDF03]
                                    ,POSMI.[UDF04]
                                    ,POSMI.[UDF05]
                                    ,POSMI.[UDF06]
                                    ,POSMI.[UDF07]
                                    ,POSMI.[UDF08]
                                    ,POSMI.[UDF09]
                                    ,POSMI.[UDF10]
                                    ,(
                                    SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(10) 
                                    FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                    WHERE MF004=MA001 AND MF003 = MI003
                                    FOR XML PATH('')) AS All_MF004
                                    ,(
                                    SELECT  LTRIM(RTRIM(NI002)) + CHAR(10)
                                    FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                    WHERE MG005=NI001 AND MG003 = MI003
                                    FOR XML PATH('')) AS All_NI002
                                    ,(
                                    SELECT  LTRIM(RTRIM(MJ004))+ CHAR(10)+LTRIM(RTRIM(MB002))+ CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MI017))+ CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MI018)) + CHAR(10) 
                                    FROM [TK].dbo.POSMJ,[TK].dbo.INVMB
                                    WHERE MJ004=MB001 AND  MJ003 = MI003
                                    FOR XML PATH('')) AS All_MC004
                                    ,(MI005+'~'+MI006) AS 'MB012MB013'
                                    ,[TB_EB_USER].USER_GUID,NAME
                                 

                                    FROM [TK].dbo.POSMI
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= POSMI.CREATOR COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE 1=1    
                                    AND MI003='{0}'
                                    ) AS TEMP
                              
                                    ", MI003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }
    public DataTable SEARCH_POSMO(string MO003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                 SELECT TEMP.*
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'

                                    FROM 
                                    (
                                    SELECT
                                    POSMO.[CREATOR]
                                    ,POSMO.[MO001]
                                    ,POSMO.[MO002]
                                    ,POSMO.[MO003]
                                    ,POSMO.[MO004]
                                    ,POSMO.[MO005]
                                    ,POSMO.[MO006]
                                    ,POSMO.[MO007]
                                    ,POSMO.[MO008]
                                    ,POSMO.[MO009]
                                    ,POSMO.[MO010]
                                    ,POSMO.[MO011]
                                    ,POSMO.[MO012]
                                    ,POSMO.[MO013]
                                    ,POSMO.[MO014]
                                    ,POSMO.[MO015]
                                    ,POSMO.[MO016]
                                    ,POSMO.[MO017]
                                    ,POSMO.[MO018]
                                    ,POSMO.[MO019]
                                    ,POSMO.[MO020]
                                    ,POSMO.[MO021]
                                    ,POSMO.[MO022]
                                    ,POSMO.[MO023]
                                    ,POSMO.[MO024]
                                    ,POSMO.[MO025]
                                    ,POSMO.[MO026]
                                    ,POSMO.[MO027]
                                    ,POSMO.[MO028]
                                    ,POSMO.[MO029]
                                    ,POSMO.[MO030]
                                    ,POSMO.[MO031]
                                    ,POSMO.[MO032]
                                    ,POSMO.[MO033]
                                    ,POSMO.[MO034]
                                    ,POSMO.[MO035]
                                    ,POSMO.[MO036]
                                    ,POSMO.[MO037]
                                    ,POSMO.[MO038]
                                    ,POSMO.[MO039]
                                    ,POSMO.[MO040]
                                    ,POSMO.[MO041]
                                    ,POSMO.[MO042]
                                    ,POSMO.[MO200]
                                    ,POSMO.[UDF01]
                                    ,POSMO.[UDF02]
                                    ,POSMO.[UDF03]
                                    ,POSMO.[UDF04]
                                    ,POSMO.[UDF05]
                                    ,POSMO.[UDF06]
                                    ,POSMO.[UDF07]
                                    ,POSMO.[UDF08]
                                    ,POSMO.[UDF09]
                                    ,POSMO.[UDF10]
                                    ,(
                                    SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(10) 
                                    FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                    WHERE MF004=MA001 AND MF003 = MO003
                                    FOR XML PATH('')) AS All_MF004
                                    ,(
                                    SELECT  LTRIM(RTRIM(NI002)) + CHAR(10)
                                    FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                    WHERE MG005=NI001 AND MG003 = MO003
                                    FOR XML PATH('')) AS All_NI002
                                    ,(
                                    SELECT  LTRIM(RTRIM(MP005))+ CHAR(10)+LTRIM(RTRIM(MB002))+ CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MP006))+ CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MP007))+ CHAR(10) 
                                    FROM [TK].dbo.POSMP,[TK].dbo.INVMB
                                    WHERE MP005=MB001 AND  MP003 = MO003
                                    FOR XML PATH('')) AS All_MC004
                                     ,(MO011+'~'+MO012) AS 'MB012MB013'
                                    ,[TB_EB_USER].USER_GUID,NAME


                                    FROM [TK].dbo.POSMO
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= POSMO.CREATOR COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE 1=1    
                                    AND MO003='{0}'
                                    ) AS TEMP
                              
                                    ", MO003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }
    public DataTable SEARCH_POSMM(string MM003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                 SELECT TEMP.*
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'

                                    FROM 
                                    (
                                    SELECT
                                    POSMM.[CREATOR]

                                    ,POSMM.[MM001]
                                    ,POSMM.[MM002]
                                    ,POSMM.[MM003]
                                    ,POSMM.[MM004]
                                    ,POSMM.[MM005]
                                    ,POSMM.[MM006]
                                    ,POSMM.[MM007]
                                    ,POSMM.[MM008]
                                    ,POSMM.[MM009]
                                    ,POSMM.[MM010]
                                    ,POSMM.[MM011]
                                    ,POSMM.[MM012]
                                    ,POSMM.[MM013]
                                    ,POSMM.[MM014]
                                    ,POSMM.[MM015]
                                    ,POSMM.[MM016]
                                    ,POSMM.[MM017]
                                    ,POSMM.[MM018]
                                    ,POSMM.[MM019]
                                    ,POSMM.[MM020]
                                    ,POSMM.[MM021]
                                    ,POSMM.[MM022]
                                    ,POSMM.[MM023]
                                    ,POSMM.[MM024]
                                    ,POSMM.[MM025]
                                    ,POSMM.[MM026]
                                    ,POSMM.[MM027]
                                    ,POSMM.[MM028]
                                    ,POSMM.[MM029]
                                    ,POSMM.[MM030]
                                    ,POSMM.[MM031]
                                    ,POSMM.[MM032]
                                    ,POSMM.[MM033]
                                    ,POSMM.[MM034]
                                    ,POSMM.[MM035]
                                    ,POSMM.[MM036]
                                    ,POSMM.[MM037]
                                    ,POSMM.[MM038]
                                    ,POSMM.[MM039]
                                    ,POSMM.[MM040]
                                    ,POSMM.[MM041]
                                    ,POSMM.[MM042]
                                    ,POSMM.[MM200]
                                    ,POSMM.[UDF01]
                                    ,POSMM.[UDF02]
                                    ,POSMM.[UDF03]
                                    ,POSMM.[UDF04]
                                    ,POSMM.[UDF05]
                                    ,POSMM.[UDF06]
                                    ,POSMM.[UDF07]
                                    ,POSMM.[UDF08]
                                    ,POSMM.[UDF09]
                                    ,POSMM.[UDF10]
                                    ,(
                                    SELECT  LTRIM(RTRIM(MF004))+LTRIM(RTRIM(MA002))+ CHAR(10) 
                                    FROM [TK].dbo.POSMF,[TK].dbo.WSCMA
                                    WHERE MF004=MA001 AND MF003 = MM003
                                    FOR XML PATH('')) AS All_MF004
                                    ,(
                                    SELECT  LTRIM(RTRIM(NI002)) + CHAR(10)
                                    FROM [TK].dbo.POSMG,[TK].dbo.WSCNI
                                    WHERE MG005=NI001 AND MG003 = MM003
                                    FOR XML PATH('')) AS All_NI002
                                    ,(
                                    SELECT  LTRIM(RTRIM(CONVERT(INT,MN005)))+' 以上'+ CHAR(10) +'非會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MN008))+ CHAR(10) +' 會員特價'+CONVERT(NVARCHAR,CONVERT(INT,MN009))+ CHAR(10) 
                                    FROM [TK].dbo.POSMN
                                    WHERE  MN003 = MM003
                                    FOR XML PATH('')) AS All_MC004
                                    ,(MM005+'~'+MM005) AS 'MB012MB013'
                                    ,[TB_EB_USER].USER_GUID,NAME


                                    FROM [TK].dbo.POSMM
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= POSMM.CREATOR COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE 1=1    
                                    AND MM003='{0}'
                                    ) AS TEMP
                              
                                    ", MM003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }
    public DataTable SEARCHUOFDEP(string ACCOUNT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                    SELECT 
                                    [GROUP_NAME] AS 'DEPNAME'
                                    ,[TB_EB_EMPL_DEP].[GROUP_ID]+','+[GROUP_NAME]+',False' AS 'DEPNO'
                                    ,[TB_EB_USER].[USER_GUID]
                                    ,[ACCOUNT]
                                    ,[NAME]
                                    ,[TB_EB_EMPL_DEP].[GROUP_ID]
                                    ,[TITLE_ID]     
                                    ,[GROUP_NAME]
                                    ,[GROUP_CODE]
                                    FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER],[192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP],[192.168.1.223].[UOF].[dbo].[TB_EB_GROUP]
                                    WHERE [TB_EB_USER].[USER_GUID]=[TB_EB_EMPL_DEP].[USER_GUID]
                                    AND [TB_EB_EMPL_DEP].[GROUP_ID]=[TB_EB_GROUP].[GROUP_ID]
                                    AND ISNULL([TB_EB_GROUP].[GROUP_CODE],'')<>''
                                    AND [ACCOUNT]='{0}'
                              
                                    ", ACCOUNT);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }
    /// <summary>
    /// POSMB 商品促銷設定單頭檔
    /// POSMI 組合品搭贈設定單頭檔
    /// POSMO 配對搭贈設定單頭檔
    /// </summary>
    /// <param name="MB003"></param>
    /// <returns></returns>
    public DataTable CHECK_POSMB_POSMI_POSMO(string MB003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"                                    
                               --20240126 查POS重疊活動-商品、日期起、門市

                                SELECT *
                                FROM 
                                (
                                SELECT MB001,MB003,MB002,MB012,MB013,MC004,MF004,MB2MB001,MB2MB003,MB2MB012,MB2MB013,MC2MC004,MF2MF004
                                FROM [TK].dbo.POSMB
                                INNER JOIN [TK].dbo.POSMC ON MB003 = MC003
                                LEFT JOIN [TK].dbo.POSMF ON MF003=MB003
                                INNER JOIN
                                (    
                                SELECT MB2.MB001 AS MB2MB001,MB2.MB003 AS MB2MB003,MB2.MB002 AS MB2MB002,MB2.MB012 AS MB2MB012 ,MB2.MB013 AS MB2MB013, MC2.MC004 AS MC2MC004,MF2.MF004 AS'MF2MF004'
                                FROM [TK].dbo.POSMB AS MB2
                                INNER JOIN [TK].dbo.POSMC AS MC2 ON MB2.MB003 = MC2.MC003
                                LEFT JOIN [TK].dbo.POSMF AS MF2 ON MF2.MF003=MB2.MB003
                                ) 
                                AS TEMP  ON  TEMP.MC2MC004 IS NOT NULL
                                AND TEMP.MB2MB003 <> POSMB.MB003
                                AND TEMP.MC2MC004 = POSMC.MC004
                                AND ((TEMP.MF2MF004=POSMF.MF004) OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')<>'') OR (ISNULL(TEMP.MF2MF004,'')<>'' AND ISNULL(POSMF.MF004,'')='') OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')=''))
                                AND 
                                (
                                MB012 BETWEEN MB2MB012 AND MB2MB013
                                OR MB013 BETWEEN MB2MB012 AND MB2MB013
                                OR (MB2MB012 BETWEEN MB012 AND MB013)
                                OR (MB2MB013 BETWEEN MB012 AND MB013)
                                )
                                WHERE MC004 IS NOT NULL
                                AND MB003 = '{0}'
                                UNION ALL

                                SELECT MI001,MI003,MI002,MI005,MI006,MJ004,MF004,MI2MI001,MI2MI003,MI2MI005,MI2MI006,MJ2MJ004,MF2MF004
                                FROM [TK].dbo.POSMI
                                INNER JOIN [TK].dbo.POSMJ ON MI003 = MJ003
                                LEFT JOIN [TK].dbo.POSMF ON MF003=MI003
                                INNER JOIN
                                (    
                                SELECT MI2.MI001 AS MI2MI001,MI2.MI003 AS MI2MI003,MI2.MI002 AS MI2MI002,MI2.MI005 AS MI2MI005 ,MI2.MI006 AS MI2MI006, MJ2.MJ004 AS MJ2MJ004,MF2.MF004 AS'MF2MF004'
                                FROM [TK].dbo.POSMI AS MI2
                                INNER JOIN [TK].dbo.POSMJ AS MJ2 ON MI2.MI003 = MJ2.MJ003
                                LEFT JOIN [TK].dbo.POSMF AS MF2 ON MF2.MF003=MI2.MI003
                                ) 
                                AS TEMP  ON  TEMP.MJ2MJ004 IS NOT NULL
                                AND TEMP.MI2MI003 <> POSMI.MI003
                                AND TEMP.MJ2MJ004 = POSMJ.MJ004
                                AND ((TEMP.MF2MF004=POSMF.MF004) OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')<>'') OR (ISNULL(TEMP.MF2MF004,'')<>'' AND ISNULL(POSMF.MF004,'')='') OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')=''))
                                AND 
                                (
                                MI005 BETWEEN MI2MI005 AND MI2MI006
                                OR MI006 BETWEEN MI2MI005 AND MI2MI006
                                OR (MI2MI005 BETWEEN MI005 AND MI006)
                                OR (MI2MI006 BETWEEN MI005 AND MI006)
                                )
                                WHERE MJ004 IS NOT NULL
                                AND MI003 = '{0}'

                                UNION ALL
                                SELECT MO001,MO003,MO002,MO011,MO012,MP005,MF004,MO2MO001,MO2MO003,MO2MO011,MO2MO012,MP2MP005,MF2MF004
                                FROM [TK].dbo.POSMO
                                INNER JOIN [TK].dbo.POSMP ON MO003 = MP003
                                LEFT JOIN [TK].dbo.POSMF ON MF003=MO003
                                INNER JOIN
                                (    
                                SELECT MO2.MO001 AS MO2MO001,MO2.MO003 AS MO2MO003,MO2.MO002 AS MO2MO002,MO2.MO011 AS MO2MO011 ,MO2.MO012 AS MO2MO012, MP2.MP005 AS MP2MP005,MF2.MF004 AS'MF2MF004'
                                FROM [TK].dbo.POSMO AS MO2
                                INNER JOIN [TK].dbo.POSMP AS MP2 ON MO2.MO003 = MP2.MP003
                                LEFT JOIN [TK].dbo.POSMF AS MF2 ON MF2.MF003=MO2.MO003
                                ) 
                                AS TEMP  ON  TEMP.MP2MP005 IS NOT NULL
                                AND TEMP.MO2MO003 <> POSMO.MO003
                                AND TEMP.MP2MP005 = POSMP.MP005
                                AND ((TEMP.MF2MF004=POSMF.MF004) OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')<>'') OR (ISNULL(TEMP.MF2MF004,'')<>'' AND ISNULL(POSMF.MF004,'')='') OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')=''))
                                AND 
                                (
                                MO011 BETWEEN MO2MO011 AND MO2MO012
                                OR MO012 BETWEEN MO2MO011 AND MO2MO012
                                OR (MO2MO011 BETWEEN MO011 AND MO012)
                                OR (MO2MO012 BETWEEN MO011 AND MO012)
                                )
                                WHERE MP005 IS NOT NULL
                                AND MO003 =  '{0}'
                                ) AS TEMP


                              
                                    ", MB003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        { }
    }
  
    
  
    public void ADDTACK(XmlElement Form)
    {
        Ede.Uof.WKF.Utility.TaskUtilityUCO taskUCO = new Ede.Uof.WKF.Utility.TaskUtilityUCO();

        string result = taskUCO.WebService_CreateTask(Form.OuterXml);

        XElement resultXE = XElement.Parse(result);

        string status = "";
        string formNBR = "";
        string error = "";

        string NEWTASK_ID = "";

        if (resultXE.Element("Status").Value == "1")
        {
            status = "送單成功!";
            formNBR = resultXE.Element("FormNumber").Value;

            NEWTASK_ID = formNBR;

            Logger.Write("起單", status + formNBR);
            MsgBox("送單成功 \r\n" + MB003 + " > " + formNBR, this.Page, this);

        }
        else
        {
            status = "送單失敗!";
            error = resultXE.Element("Exception").Element("Message").Value;

            Logger.Write("起單", status + error + "\r\n" + Form.OuterXml);

            MsgBox("失敗了，無法送單!!!!    " + error + "\r\n" + Form.OuterXml, this.Page, this);

            throw new Exception(status + error + "\r\n" + Form.OuterXml);

        }
    }
    public void MsgBox(String ex, Page pg, Object obj)
    {
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }
    #endregion

    #region BUTTON
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();       

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        BindGrid2();

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        BindGrid3();

    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        BindGrid4();

    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        BindGrid5();

    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        BindGrid6();

    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        BindGrid7();
    }
    #endregion
}