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

public partial class CDS_WebPage_COP_TBBU_TBCOPTFCHECK_BAKING : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    string DBNAME = "UOF";
    //string DBNAME = "UOFTEST";
    string COPCHANGEID;

    string TF001 = "";
    string TF002 = "";
    string TF003 = "";
    string COPTCUDF01 = "N";
    string COPCALL1 = "N";

    protected void Page_Load(object sender, EventArgs e)
    {

        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        ROLES = SEARCHROLES(ACCOUNT.Trim());

        SETBUTTON();
        RadPageView6.Visible = false;


        if (ROLES.Equals("ADMIN"))
        {
            Button4.Enabled = true;
            Button6.Enabled = true;
            Button8.Enabled = true;

            RadPageView6.Visible = true;

        }

        else if (ROLES.Equals("MOC"))
        {
            Button4.Enabled = true;
            Button6.Enabled = false;
            Button8.Enabled = false;

        }
        else if (ROLES.Equals("PUR"))
        {
            Button4.Enabled = false;
            Button6.Enabled = true;
            Button8.Enabled = false;

        }
        else if (ROLES.Equals("SLAES"))
        {
            Button4.Enabled = false;
            Button6.Enabled = false;
            Button8.Enabled = true;

        }

        if (!IsPostBack)
        {
            TextBox15.Text = DateTime.Now.ToString("yyyyMMdd");
            

            SETDATES();

            BindDropDownList();
            BindDropDownList2();
            BindGrid("");

            BindDropDownList3();
            BindDropDownList4();
            BindGrid2("");

            BindDropDownList5();
            BindDropDownList6();
            //BindGrid3("");

            BindDropDownList7();
            BindDropDownList8();
            //BindGrid4("");

            BindDropDownList9();
            BindDropDownList10();

            //BindGrid5("");

            BindGrid7("");
        }
        else
        {

        }




    }
    #region FUNCTION
    public void SETBUTTON()
    {
        Button4.Enabled = false;      
    }
    public void SETDATES()
    {
        TextBox1.Text = DateTime.Now.ToString("yyyy");
        //TextBox2.Text = DateTime.Now.ToString("MM");
        TextBox3.Text = DateTime.Now.ToString("yyyy");
        //TextBox4.Text = DateTime.Now.ToString("MM");
        TextBox5.Text = DateTime.Now.ToString("yyyy");
        //TextBox6.Text = DateTime.Now.ToString("MM");
        TextBox7.Text = DateTime.Now.ToString("yyyy");
        //TextBox8.Text = DateTime.Now.ToString("MM");
        TextBox13.Text = DateTime.Now.ToString("yyyy");
        //TextBox14.Text = DateTime.Now.ToString("MM");

        TextBox26.Text = DateTime.Now.ToString("yyyy");

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

        string cmdTxt = @" SELECT 'Y' AS 'STATUS' UNION ALL SELECT 'N' AS 'STATUS' ";

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

        string cmdTxt = @" SELECT 'Y' AS 'STATUS' UNION ALL SELECT 'N' AS 'STATUS' ";

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

        string cmdTxt = @" SELECT 'Y' AS 'STATUS' UNION ALL SELECT 'N' AS 'STATUS' ";

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

    private void BindDropDownList8()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT 'Y' AS 'STATUS' UNION ALL SELECT 'N' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList8.DataSource = dt;
            DropDownList8.DataTextField = "STATUS";
            DropDownList8.DataValueField = "STATUS";
            DropDownList8.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList9()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList9.DataSource = dt;
            DropDownList9.DataTextField = "STATUS";
            DropDownList9.DataValueField = "STATUS";
            DropDownList9.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList10()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList10.DataSource = dt;
            DropDownList10.DataTextField = "STATUS";
            DropDownList10.DataValueField = "STATUS";
            DropDownList10.DataBind();

        }
        else
        {

        }



    }




    private void BindGrid(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox1.Text) )
        {
           
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%' ", TextBox1.Text.Trim() );

        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TE029='N' ");
            }
            else if (DropDownList1.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TE029='Y' ");
            }
        }


        //是否生產
        if (!string.IsNullOrEmpty(DropDownList2.Text))
        {
            if (DropDownList2.Text.Equals("Y"))
            {
                QUERYS.AppendFormat(@" AND COPTF.UDF01 IN ('Y','y') ");
            }
            else if (DropDownList2.Text.Equals("N"))
            {
                QUERYS.AppendFormat(@" AND COPTF.UDF01 NOT IN ('Y','y')  ");
            }
        }

        //訂單單號
        if (!string.IsNullOrEmpty(TextBox9.Text))
        {            
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox9.Text.Trim());

        }

        //客戶名稱
        if (!string.IsNullOrEmpty(TextBox24.Text))
        {
            QUERYS.AppendFormat(@" AND TC053 LIKE '%{0}%'", TextBox24.Text.Trim());

        }

        //品名     
        if (!string.IsNullOrEmpty(TextBox25.Text))
        {
            QUERYS.AppendFormat(@" AND TD005 LIKE '%{0}%'", TextBox25.Text.Trim());

        }

        //過濾烘培品
        DataTable DT = SEARCH_MOCMANULINEMB001LIKES();
        if (DT != null && DT.Rows.Count >= 1)
        {
            QUERYS2.AppendFormat(" AND (");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (i > 0) // Add OR after the first condition
                {
                    QUERYS2.AppendFormat(" OR ");
                }
                QUERYS2.AppendFormat("TF005 LIKE '{0}%'", DT.Rows[i]["MB001"].ToString());
            }
            QUERYS2.AppendFormat(")");
        }
        else
        {
            // No additional SQL clause required
            QUERYS2.AppendFormat("");
        }

        cmdTxt.AppendFormat(@" 
                                SELECT 
                                LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,*
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
                                ,COPTF.UDF01 AS 'COPTFUDF01'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                AND 1=1
                                {0}
                                {1}
                                

                                ", QUERYS.ToString(), QUERYS2.ToString());



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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ///Get the button that raised the event
            Button btn = (Button)e.Row.FindControl("Button1");
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue = btn.CommandArgument;
            DataRowView row = (DataRowView)e.Row.DataItem;
            Button lbtnName = (Button)e.Row.FindControl("Button1");
            ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            // Dialog.PostBackType.AfterReturn
            //Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            // Dialog.PostBackType.Allows
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COWORK/TBBU_TBCOPTFCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.Allows, param);


            //Button2
            //Get the button that raised the event
            Button btn2 = (Button)e.Row.FindControl("Button2");
            //Get the row that contains this button
            GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue2 = btn2.CommandArgument;
            DataRowView row2 = (DataRowView)e.Row.DataItem;
            Button lbtnName2 = (Button)e.Row.FindControl("Button2");
            ExpandoObject param2 = new { ID = Cellvalue }.ToExpando();
            


        }





        //StringBuilder PATH = new StringBuilder();

        //System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");



        //    if (!string.IsNullOrEmpty(row["PHOTO_GUID"].ToString()))
        //    {
        //        //img.ImageUrl = "https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id=8b2a033b-c301-419b-938d-e6cfedf28b82&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name=40100010650490.png";


        //        //PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id={0}&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name={1}
        //        //                ", row["RESIZE_FILE_ID"].ToString(), row["PHOTO_DESC"].ToString());

        //        PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/Common/FileCenter/V3/Handler/FileControlHandler.ashx?id={0}
        //                        ", row["RESIZE_FILE_ID"].ToString());
        //        img.ImageUrl = PATH.ToString();

        //        //img.ImageUrl  = Request.ApplicationPath + "/Common/FileCenter/ShowImage.aspx?id=" + row["THUMBNAIL_FILE_ID"].ToString();

        //        //img.ImageUrl = string.Format("~/Common/FileCenter/Downloadfile.ashx?id={0}", row["THUMBNAIL_FILE_ID"].ToString());

        //        //e.Row.Cells[0].Text = row["THUMBNAIL_FILE_ID"].ToString();
        //        ////獲取當前行的圖片路徑
        //        //string ImgUrl = img.ImageUrl;
        //        ////給帶圖片的單元格添加點擊事件
        //        //e.Row.Cells[3].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
        //        //    + ".checked=true;CellClick('" + ImgUrl + "')");

        //        //  img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
        //    }


        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label Label_COPTFUDF01 = (Label)e.Row.FindControl("Label_COPTFUDF01");
            Label Label_PURCHECKS = (Label)e.Row.FindControl("Label_PURCHECKS");
            Label Label_MOCCHECKS = (Label)e.Row.FindControl("Label_MOCCHECKS");
            Button Button2 = (Button)e.Row.FindControl("Button2");
            if(Label_COPTFUDF01.Text.Equals("Y"))
            {
                if (Label_PURCHECKS.Text.Equals("Y") && Label_MOCCHECKS.Text.Equals("Y"))
                {
                    Button2.Visible = true;
                }
                else
                {
                    Button2.Visible = false;
                }
            }
            else
            {
                Button2.Visible = true;
            }
          
        }

    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Button1")
        {
            //MsgBox("Button1", this.Page, this);
            BindGrid("");
        }
        else if (e.CommandName == "Button2")
        {
            //檢查是否已送單，簽核中
            DataTable DT = CHECK_TB_WKF_TASK(e.CommandArgument.ToString());

            if (DT != null && DT.Rows.Count >= 1)
            {
                MsgBox(e.CommandArgument.ToString() + "重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
            }
            else
            {
                //檢查並送UOF
                CHECKTBCOPTFCHECK(e.CommandArgument.ToString());

                //用訂單變更找出客代TC004
                DataTable DTCOPTE = FINDCOPTE(e.CommandArgument.ToString());
                string TC004 = DTCOPTE.Rows[0]["TE007"].ToString();
                //訂單金額
                decimal COPTCMONEYS = Convert.ToDecimal(DTCOPTE.Rows[0]["COPTCMONEYS"].ToString());
                int INTCOPTCMONEYS = Convert.ToInt32(COPTCMONEYS);
                //訂單變更金額
                decimal COPTEMONEYS = Convert.ToDecimal(DTCOPTE.Rows[0]["COPTEMONEYS"].ToString());
                int INTCOPTEMONEYS = Convert.ToInt32(COPTEMONEYS);
                //用客代找出信用額度設定上限
                decimal TOTALLIMITS = FINDCOPMATOTALLIMITS(TC004);
                int INTTOTALLIMITS = Convert.ToInt32(TOTALLIMITS);
                //用客代找出目前已用的信用額度
                decimal TOTALCREDITS = FINDCREDITS(TC004);
                int INTTOTALCREDITS = Convert.ToInt32(TOTALCREDITS);

                if (INTTOTALLIMITS < (INTCOPTEMONEYS + (-1 * INTTOTALCREDITS) + INTCOPTCMONEYS))
                {
                    MsgBox(e.CommandArgument.ToString() + " 訂單變更金額=" + INTCOPTEMONEYS.ToString("0,0") + " \r\n原訂單金額=" + INTCOPTCMONEYS.ToString("0,0") + " \r\n客代:" + TC004 + " \r\n目前設定的信用額度=" + INTTOTALLIMITS.ToString("0,0") + " \r\n已花費的信用額度=" + INTTOTALCREDITS.ToString("0,0"), this.Page, this);
                }

            }


            //MsgBox(e.CommandArgument.ToString(), this.Page, this);           
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

    //private void AddImage(ExcelWorksheet oSheet, int rowIndex, int colIndex, string imagePath)
    //{
    //    Bitmap image = new Bitmap(imagePath);
    //    ExcelPicture excelImage = null;
    //    if (image != null)
    //    {
    //        excelImage = oSheet.Drawings.AddPicture("Debopam Pal", image);
    //        excelImage.From.Column = colIndex;
    //        excelImage.From.Row = rowIndex;
    //        excelImage.SetSize(100, 100);
    //        //2x2 px space for better alignment
    //        excelImage.From.ColumnOff = Pixel2MTU(2);
    //        excelImage.From.RowOff = Pixel2MTU(2);
    //    }
    //}

    //public int Pixel2MTU(int pixels)
    //{
    //    int mtus = pixels * 9525;
    //    return mtus;
    //}

    private void BindGrid2(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox3.Text) )
        {
           
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox3.Text.Trim() );

        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList3.Text))
        {
            if (DropDownList3.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TE029='N'");
            }
            else if (DropDownList3.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TE029='Y'");
            }
        }


        //是否生產
        if (!string.IsNullOrEmpty(DropDownList4.Text))
        {
            if (DropDownList4.Text.Equals("Y"))
            {
                QUERYS.AppendFormat(@" AND COPTF.UDF01 IN ('Y','y') ");
            }
            else if (DropDownList4.Text.Equals("N"))
            {
                QUERYS.AppendFormat(@" AND COPTF.UDF01 NOT IN ('Y','y')  ");
            }
        }

        //訂單單號
        if (!string.IsNullOrEmpty(TextBox10.Text))
        {
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox10.Text.Trim());

        }

        //客戶名稱
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS.AppendFormat(@" AND TC053 LIKE '%{0}%'", TextBox2.Text.Trim());

        }

        //品名     
        if (!string.IsNullOrEmpty(TextBox6.Text))
        {
            QUERYS.AppendFormat(@" AND TD005 LIKE '%{0}%'", TextBox6.Text.Trim());

        }
        //過濾烘培品
        DataTable DT = SEARCH_MOCMANULINEMB001LIKES();
        if (DT != null && DT.Rows.Count >= 1)
        {
            QUERYS2.AppendFormat(" AND (");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (i > 0) // Add OR after the first condition
                {
                    QUERYS2.AppendFormat(" OR ");
                }
                QUERYS2.AppendFormat("TF005 LIKE '{0}%'", DT.Rows[i]["MB001"].ToString());
            }
            QUERYS2.AppendFormat(")");
        }
        else
        {
            // No additional SQL clause required
            QUERYS2.AppendFormat("");
        }

        cmdTxt.AppendFormat(@" 
                                
                                SELECT 
                                LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,*
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
                                ,COPTF.UDF01 AS 'COPTFUDF01'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                AND 1=1
                                {0}
                                {1}

                                ", QUERYS.ToString(), QUERYS2.ToString());




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
            DropDownList ddl = (DropDownList)e.Row.FindControl("GRIDVIEWDropDownList1");
            if (ddl != null)
            {
                // 获取当前行的Approval值
                string MOCCHECKS = DataBinder.Eval(e.Row.DataItem, "MOCCHECKS").ToString();
                ddl.SelectedValue = MOCCHECKS;
            }

        }

    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Button3")
        {
            //MsgBox("Button1", this.Page, this);
            BindGrid2("");
        }
        else if (e.CommandName == "Button4")
        {
            CHECKTBCOPTFCHECK(e.CommandArgument.ToString());
            //MsgBox(e.CommandArgument.ToString(), this.Page, this);           
        }

    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

        
    }

    private void BindGrid3(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox5.Text) )
        {
          
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox5.Text.Trim());

        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList5.Text))
        {
            if (DropDownList5.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TE029='N'");
            }
            else if (DropDownList5.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TE029='Y'");
            }
        }


        //是否生產
        if (!string.IsNullOrEmpty(DropDownList6.Text))
        {
            if (DropDownList6.Text.Equals("Y"))
            {
                QUERYS.AppendFormat(@" AND COPTF.UDF01 IN ('Y','y') ");
            }
            else if (DropDownList6.Text.Equals("N"))
            {
                QUERYS.AppendFormat(@" AND COPTF.UDF01 NOT IN ('Y','y')  ");
            }
        }

        //訂單單號
        if (!string.IsNullOrEmpty(TextBox11.Text))
        {
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox11.Text.Trim());

        }


        //客戶名稱
        if (!string.IsNullOrEmpty(TextBox8.Text))
        {
            QUERYS.AppendFormat(@" AND TC053 LIKE '%{0}%'", TextBox8.Text.Trim());

        }

        //品名     
        if (!string.IsNullOrEmpty(TextBox14.Text))
        {
            QUERYS.AppendFormat(@" AND TD005 LIKE '%{0}%'", TextBox14.Text.Trim());

        }

        //過濾烘培品
        DataTable DT = SEARCH_MOCMANULINEMB001LIKES();
        if (DT != null && DT.Rows.Count >= 1)
        {
            QUERYS2.AppendFormat(" AND (");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (i > 0) // Add OR after the first condition
                {
                    QUERYS2.AppendFormat(" OR ");
                }
                QUERYS2.AppendFormat("TF005 LIKE '{0}%'", DT.Rows[i]["MB001"].ToString());
            }
            QUERYS2.AppendFormat(")");
        }
        else
        {
            // No additional SQL clause required
            QUERYS2.AppendFormat("");
        }

        cmdTxt.AppendFormat(@" 
                                SELECT 
                                LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,*
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
                                ,COPTF.UDF01 AS 'COPTFUDF01'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002  AND TE003=TF003
                                AND 1=1
                                {0}
                                {1}

                                ", QUERYS.ToString(), QUERYS2.ToString());




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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("GRIDVIEW2DropDownList1");
                if (ddl != null)
                {
                    // 获取当前行的Approval值
                    string PURCHECKS = DataBinder.Eval(e.Row.DataItem, "PURCHECKS").ToString();
                    ddl.SelectedValue = PURCHECKS;
                }

            }

        }

    }

    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //int rowIndex = -1;

        //if (e.CommandName == "Button3")
        //{
        //    //MsgBox("Button1", this.Page, this);
        //    BindGrid3("");
        //}
        //else if (e.CommandName == "Button4")
        //{
        //    CHECKTBCOPTDCHECK(e.CommandArgument.ToString());
        //    //MsgBox(e.CommandArgument.ToString(), this.Page, this);           
        //}

    }


    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();


    }

    private void BindGrid4(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        ////日期
        //if (!string.IsNullOrEmpty(TextBox7.Text) && !string.IsNullOrEmpty(TextBox8.Text))
        //{
        //    if (TextBox8.Text.Length == 1)
        //    {
        //        TextBox8.Text = "0" + TextBox8.Text;
        //    }
        //    QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox7.Text.Trim() + TextBox8.Text.Trim());

        //}

        //日期
        if (!string.IsNullOrEmpty(TextBox7.Text) )
        {
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox7.Text.Trim() );

        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList7.Text))
        {
            if (DropDownList7.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TE029='N'");
            }
            else if (DropDownList7.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TE029='Y'");
            }
        }


        //是否生產
        if (!string.IsNullOrEmpty(DropDownList8.Text))
        {
            if (DropDownList8.Text.Equals("Y"))
            {
                QUERYS.AppendFormat(@" AND COPTF.UDF01 IN ('Y','y') ");
            }
            else if (DropDownList8.Text.Equals("N"))
            {
                QUERYS.AppendFormat(@" AND COPTF.UDF01 NOT IN ('Y','y')  ");
            }
        }

        //訂單單號
        if (!string.IsNullOrEmpty(TextBox12.Text))
        {
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox12.Text.Trim());

        }


        //客戶名稱
        if (!string.IsNullOrEmpty(TextBox16.Text))
        {
            QUERYS.AppendFormat(@" AND TC053 LIKE '%{0}%'", TextBox16.Text.Trim());

        }

        //品名     
        if (!string.IsNullOrEmpty(TextBox17.Text))
        {
            QUERYS.AppendFormat(@" AND TD005 LIKE '%{0}%'", TextBox17.Text.Trim());

        }

        cmdTxt.AppendFormat(@" 
                                 SELECT 
                                LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,*
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
                                ,COPTF.UDF01 AS 'COPTFUDF01'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002  AND TE003=TF003
                                AND 1=1

                                {0}

                                

                                ", QUERYS.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //    ///Get the button that raised the event
            //    Button btn = (Button)e.Row.FindControl("Button3");
            //    //Get the row that contains this button
            //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //    //string cellvalue = gvr.Cells[2].Text.Trim();
            //    string Cellvalue = btn.CommandArgument;
            //    DataRowView row = (DataRowView)e.Row.DataItem;
            //    Button lbtnName = (Button)e.Row.FindControl("Button3");
            //    ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //    //Grid開窗是用RowDataBound事件再開窗
            //    // Dialog.PostBackType.AfterReturn
            //    //Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            //    // Dialog.PostBackType.Allows
            //    Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.Allows, param);


            //    //Button2
            //    //Get the button that raised the event
            //    Button btn2 = (Button)e.Row.FindControl("Button4");
            //    //Get the row that contains this button
            //    GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;
            //    //string cellvalue = gvr.Cells[2].Text.Trim();
            //    string Cellvalue2 = btn2.CommandArgument;
            //    DataRowView row2 = (DataRowView)e.Row.DataItem;
            //    Button lbtnName2 = (Button)e.Row.FindControl("Button4");
            //    ExpandoObject param2 = new { ID = Cellvalue }.ToExpando();



        }





        //StringBuilder PATH = new StringBuilder();

        //System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");



        //    if (!string.IsNullOrEmpty(row["PHOTO_GUID"].ToString()))
        //    {
        //        //img.ImageUrl = "https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id=8b2a033b-c301-419b-938d-e6cfedf28b82&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name=40100010650490.png";


        //        //PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id={0}&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name={1}
        //        //                ", row["RESIZE_FILE_ID"].ToString(), row["PHOTO_DESC"].ToString());

        //        PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/Common/FileCenter/V3/Handler/FileControlHandler.ashx?id={0}
        //                        ", row["RESIZE_FILE_ID"].ToString());
        //        img.ImageUrl = PATH.ToString();

        //        //img.ImageUrl  = Request.ApplicationPath + "/Common/FileCenter/ShowImage.aspx?id=" + row["THUMBNAIL_FILE_ID"].ToString();

        //        //img.ImageUrl = string.Format("~/Common/FileCenter/Downloadfile.ashx?id={0}", row["THUMBNAIL_FILE_ID"].ToString());

        //        //e.Row.Cells[0].Text = row["THUMBNAIL_FILE_ID"].ToString();
        //        ////獲取當前行的圖片路徑
        //        //string ImgUrl = img.ImageUrl;
        //        ////給帶圖片的單元格添加點擊事件
        //        //e.Row.Cells[3].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
        //        //    + ".checked=true;CellClick('" + ImgUrl + "')");

        //        //  img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
        //    }


        //}


    }

    protected void Grid4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "GRIDVIEW4Button1")
        {
            CHECKTBCOPTFCHECK(e.CommandArgument.ToString());
            //MsgBox(e.CommandArgument.ToString(), this.Page, this);           
        }

    }


    public void OnBeforeExport4(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();


    }

    private void BindGrid5(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        ////日期
        //if (!string.IsNullOrEmpty(TextBox13.Text) && !string.IsNullOrEmpty(TextBox14.Text))
        //{
        //    if (TextBox14.Text.Length == 1)
        //    {
        //        TextBox14.Text = "0" + TextBox14.Text;
        //    }
        //    QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox13.Text.Trim() + TextBox14.Text.Trim());

        //}

        //日期
        if (!string.IsNullOrEmpty(TextBox13.Text) )
        {
          
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox13.Text.Trim());

        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList9.Text))
        {
            if (DropDownList9.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TE029='N'");
            }
            else if (DropDownList9.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TE029='Y'");
            }
        }


        ////是否生產
        //if (!string.IsNullOrEmpty(DropDownList10.Text))
        //{
        //    if (DropDownList10.Text.Equals("Y"))
        //    {
        //        QUERYS.AppendFormat(@" AND COPTF.UDF01 IN ('Y','y') ");
        //    }
        //    else if (DropDownList10.Text.Equals("N"))
        //    {
        //        //QUERYS.AppendFormat(@" AND COPTF.UDF01 NOT IN ('Y','y')  ");
        //    }
        //}

        //訂單單號
        if (!string.IsNullOrEmpty(TextBox15.Text))
        {
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox15.Text.Trim());

        }


        //客戶名稱
        if (!string.IsNullOrEmpty(TextBox18.Text))
        {
            QUERYS.AppendFormat(@" AND TC053 LIKE '%{0}%'", TextBox18.Text.Trim());

        }

        //品名     
        if (!string.IsNullOrEmpty(TextBox19.Text))
        {
            QUERYS.AppendFormat(@" AND TD005 LIKE '%{0}%'", TextBox19.Text.Trim());

        }

        cmdTxt.AppendFormat(@" 
                                SELECT 
                                LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,*
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL([MOCCHECKDATES],'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
                                ,COPTF.UDF01 AS 'COPTFUDF01'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002  AND TE003=TF003
                                AND 1=1
                               
                                {0}

                               

                                ", QUERYS.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid5.DataSource = dt;
        Grid5.DataBind();
    }

    protected void grid_PageIndexChanging5(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid5_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //    ///Get the button that raised the event
            //    Button btn = (Button)e.Row.FindControl("Button3");
            //    //Get the row that contains this button
            //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //    //string cellvalue = gvr.Cells[2].Text.Trim();
            //    string Cellvalue = btn.CommandArgument;
            //    DataRowView row = (DataRowView)e.Row.DataItem;
            //    Button lbtnName = (Button)e.Row.FindControl("Button3");
            //    ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //    //Grid開窗是用RowDataBound事件再開窗
            //    // Dialog.PostBackType.AfterReturn
            //    //Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            //    // Dialog.PostBackType.Allows
            //    Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.Allows, param);


            //    //Button2
            //    //Get the button that raised the event
            //    Button btn2 = (Button)e.Row.FindControl("Button4");
            //    //Get the row that contains this button
            //    GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;
            //    //string cellvalue = gvr.Cells[2].Text.Trim();
            //    string Cellvalue2 = btn2.CommandArgument;
            //    DataRowView row2 = (DataRowView)e.Row.DataItem;
            //    Button lbtnName2 = (Button)e.Row.FindControl("Button4");
            //    ExpandoObject param2 = new { ID = Cellvalue }.ToExpando();



        }





        //StringBuilder PATH = new StringBuilder();

        //System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");



        //    if (!string.IsNullOrEmpty(row["PHOTO_GUID"].ToString()))
        //    {
        //        //img.ImageUrl = "https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id=8b2a033b-c301-419b-938d-e6cfedf28b82&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name=40100010650490.png";


        //        //PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id={0}&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name={1}
        //        //                ", row["RESIZE_FILE_ID"].ToString(), row["PHOTO_DESC"].ToString());

        //        PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/Common/FileCenter/V3/Handler/FileControlHandler.ashx?id={0}
        //                        ", row["RESIZE_FILE_ID"].ToString());
        //        img.ImageUrl = PATH.ToString();

        //        //img.ImageUrl  = Request.ApplicationPath + "/Common/FileCenter/ShowImage.aspx?id=" + row["THUMBNAIL_FILE_ID"].ToString();

        //        //img.ImageUrl = string.Format("~/Common/FileCenter/Downloadfile.ashx?id={0}", row["THUMBNAIL_FILE_ID"].ToString());

        //        //e.Row.Cells[0].Text = row["THUMBNAIL_FILE_ID"].ToString();
        //        ////獲取當前行的圖片路徑
        //        //string ImgUrl = img.ImageUrl;
        //        ////給帶圖片的單元格添加點擊事件
        //        //e.Row.Cells[3].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
        //        //    + ".checked=true;CellClick('" + ImgUrl + "')");

        //        //  img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
        //    }


        //}


    }

    protected void Grid5_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "GRIDVIEW5Button1")
        {
            CHECKTBCOPTFCHECK2(e.CommandArgument.ToString());
            //MsgBox(e.CommandArgument.ToString(), this.Page, this);           
        }

    }


    public void OnBeforeExport5(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();


    }

    private void BindGrid6(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();


       

        //訂單單號
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            QUERYS.AppendFormat(@" AND TF002 LIKE '{0}%'", TextBox4.Text.Trim());

        }

        cmdTxt.AppendFormat(@" 
                                SELECT 
                                LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,*
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL([MOCCHECKDATES],'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
                                ,COPTF.UDF01 AS 'COPTFUDF01'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002  AND TE003=TF003
                                AND 1=1

                                {0}

                               

                                ", QUERYS.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid6.DataSource = dt;
        Grid6.DataBind();
    }

    protected void grid_PageIndexChanging6(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid6_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //    ///Get the button that raised the event
            //    Button btn = (Button)e.Row.FindControl("Button3");
            //    //Get the row that contains this button
            //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //    //string cellvalue = gvr.Cells[2].Text.Trim();
            //    string Cellvalue = btn.CommandArgument;
            //    DataRowView row = (DataRowView)e.Row.DataItem;
            //    Button lbtnName = (Button)e.Row.FindControl("Button3");
            //    ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //    //Grid開窗是用RowDataBound事件再開窗
            //    // Dialog.PostBackType.AfterReturn
            //    //Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            //    // Dialog.PostBackType.Allows
            //    Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.Allows, param);


            //    //Button2
            //    //Get the button that raised the event
            //    Button btn2 = (Button)e.Row.FindControl("Button4");
            //    //Get the row that contains this button
            //    GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;
            //    //string cellvalue = gvr.Cells[2].Text.Trim();
            //    string Cellvalue2 = btn2.CommandArgument;
            //    DataRowView row2 = (DataRowView)e.Row.DataItem;
            //    Button lbtnName2 = (Button)e.Row.FindControl("Button4");
            //    ExpandoObject param2 = new { ID = Cellvalue }.ToExpando();



        }





        //StringBuilder PATH = new StringBuilder();

        //System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");



        //    if (!string.IsNullOrEmpty(row["PHOTO_GUID"].ToString()))
        //    {
        //        //img.ImageUrl = "https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id=8b2a033b-c301-419b-938d-e6cfedf28b82&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name=40100010650490.png";


        //        //PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id={0}&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name={1}
        //        //                ", row["RESIZE_FILE_ID"].ToString(), row["PHOTO_DESC"].ToString());

        //        PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/Common/FileCenter/V3/Handler/FileControlHandler.ashx?id={0}
        //                        ", row["RESIZE_FILE_ID"].ToString());
        //        img.ImageUrl = PATH.ToString();

        //        //img.ImageUrl  = Request.ApplicationPath + "/Common/FileCenter/ShowImage.aspx?id=" + row["THUMBNAIL_FILE_ID"].ToString();

        //        //img.ImageUrl = string.Format("~/Common/FileCenter/Downloadfile.ashx?id={0}", row["THUMBNAIL_FILE_ID"].ToString());

        //        //e.Row.Cells[0].Text = row["THUMBNAIL_FILE_ID"].ToString();
        //        ////獲取當前行的圖片路徑
        //        //string ImgUrl = img.ImageUrl;
        //        ////給帶圖片的單元格添加點擊事件
        //        //e.Row.Cells[3].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
        //        //    + ".checked=true;CellClick('" + ImgUrl + "')");

        //        //  img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
        //    }


        //}


    }

    protected void Grid6_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "GRIDVIEW6Button1")
        {
            CHECKTBCOPTFCHECK3(e.CommandArgument.ToString());
            //MsgBox(e.CommandArgument.ToString(), this.Page, this);           
        }

    }


    public void OnBeforeExport6(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();


    }


    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    public void SETEXCEL()
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

    public void CHECKTBCOPTFCHECK(string TF001002003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        try
        {
            // WHERE   COPTFUDF01 IN ('Y','y')
            // AND ([MOCCHECKS] NOT IN ('Y') OR [PURCHECKS] NOT IN ('Y') )
            // 訂單單身中，如果有要生產的，就一定要經生管、採購核準後，才能產生表單

            cmdTxt.AppendFormat(@"                              
                                SELECT *
                                FROM 
                                (
                                SELECT 
                                COPTF.UDF01 AS 'COPTFUDF01'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,TF001,TF002,TF003,TF004
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                AND 1=1
                                ) AS TEMP 
                                WHERE COPTFUDF01 IN ('Y','y')  
                                AND (ISNULL(MOCCHECKS,'') NOT IN ('Y') OR ISNULL(PURCHECKS,'') NOT IN ('Y') )
                                AND LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))='{0}'

                                ", TF001002003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            TF001 = TF001002003.Substring(0, 4);
            TF002 = TF001002003.Substring(4, 11);
            TF003 = TF001002003.Substring(15, 4);


            //訂單的單身有需要生產的，需經生管、採購同意
            //訂單的單身都不需要生產的，直接核單
            if (dt.Rows.Count == 0)
            {
                //檢查是否已送單，簽核中
                DataTable DT = CHECK_TB_WKF_TASK(TF001+ TF002);

                if (DT != null && DT.Rows.Count >= 1)
                {
                    MsgBox(TF001 + TF002 + "  重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
                }
                else
                {
                    ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF(TF001, TF002, TF003);
                }
               
                //MsgBox("OK" + TF001 + TF002+ TF003, this.Page, this);
            }
            else
            {
                MsgBox("送單失敗!!!! \r\n 送單失敗!!!!\r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!!       \r\n \r\n訂單的單身有需要生產的，需經生管、採購同意" + TF001 + TF002 + TF003, this.Page, this);
            }
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
        finally
        {

        }

       

    }

    public void CHECKTBCOPTFCHECK2(string TF001002003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        try
        {

            //WHERE COPTFUDF01 IN ('Y','y')，訂單單身中，含有要生產的
            //如果有要生產的，就不可以產生表單
            cmdTxt.AppendFormat(@"                              
                                SELECT *
                                FROM 
                                (
                                SELECT 
                                COPTF.UDF01 AS 'COPTFUDF01'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,TF001,TF002,TF003,TF004
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                AND 1=1
                                ) AS TEMP 
                                WHERE COPTFUDF01 IN ('Y','y')  
                                AND LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))='{0}'

                                ", TF001002003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            TF001 = TF001002003.Substring(0, 4);
            TF002 = TF001002003.Substring(4, 11);
            TF003 = TF001002003.Substring(15, 4);

            //訂單的單身有需要生產的，需經生管、採購同意
            //訂單的單身都不需要生產的，直接核單
            if (dt.Rows.Count == 0)
            {
                //檢查是否已送單，簽核中
                DataTable DT = CHECK_TB_WKF_TASK(TF001+ TF002);

                if (DT != null && DT.Rows.Count >= 1)
                {
                    MsgBox(TF001 + TF002 + " 重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
                }
                else
                {
                    ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF(TF001, TF002, TF003);
                }
               
                //MsgBox("OK" + TF001 + TF002 + TF003, this.Page, this);
            }
            else
            {
                MsgBox("送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n \r\n訂單的單身有需要生產的，需經生管、採購同意" + TF001 + TF002 + TF003, this.Page, this);
            }
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
        finally
        {

        }



    }

    public void CHECKTBCOPTFCHECK3(string TF001002003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        try
        {

            //WHERE COPTFUDF01 IN ('Y','y')，訂單單身中，含有要生產的
            //如果有要生產的，就不可以產生表單
            cmdTxt.AppendFormat(@"                              
                                SELECT *
                                FROM 
                                (
                                SELECT 
                                COPTF.UDF01 AS 'COPTFUDF01'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
                                ,TF001,TF002,TF003,TF004
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'

                                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                AND 1=1
                                ) AS TEMP 
                                WHERE  LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))='{0}'

                                ", TF001002003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            TF001 = TF001002003.Substring(0, 4);
            TF002 = TF001002003.Substring(4, 11);
            TF003 = TF001002003.Substring(15, 4);

            //訂單的單身有需要生產的，需經生管、採購同意
            //在admin強制執行，不用等生管、採購同意

            if (dt.Rows.Count > 0)
            {
                //檢查是否已送單，簽核中
                DataTable DT = CHECK_TB_WKF_TASK(TF001+ TF002);

                if (DT != null && DT.Rows.Count >= 1)
                {
                    MsgBox(TF001 + TF002 + " 重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
                }
                else
                {
                    ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF(TF001, TF002, TF003);
                }
               
                //MsgBox("OK" + TF001 + TF002 + TF003, this.Page, this);
            }
         
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
        finally
        {

        }



    }

    public void CHECKTBCOPTFCHECK7(string TF001002003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        try
        {           

            TF001 = TF001002003.Substring(0, 4);
            TF002 = TF001002003.Substring(4, 11);
            TF003 = TF001002003.Substring(15, 4);
            ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF7(TF001, TF002, TF003);

        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
        finally
        {

        }



    }


    public void ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF(string TE001, string TE002, string TE003)
    {

        DataTable DT = SEARCHCOPTECOPTF(TE001, TE002, TE003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["TE009"].ToString());
        DataTable DT_CHECK_COPTF_ZINVMBBAKING = CHECK_COPTF_ZINVMBBAKING(TE001, TE002, TE003);

        string account = DT.Rows[0]["TE009"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string BA = DT.Rows[0]["BA"].ToString();
        string BANAME = DT.Rows[0]["BANAME"].ToString();
        string BA_USER_GUID = DT.Rows[0]["BA_USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["TE001"].ToString().Trim() + DT.Rows[0]["TE002"].ToString().Trim() + DT.Rows[0]["TE003"].ToString().Trim();

        int rowscounts = 0;

        //COPTFUDF01，如果在變更單要生產的，需簽到生管
        COPTCUDF01 = "N";
        foreach (DataRow od in DT.Rows)
        {
            if (od["COPTFUDF01"].ToString().Equals("Y"))
            {
                COPTCUDF01 = "Y";
                break;
            }
            else
            {
                COPTCUDF01 = "N";
            }
        }

        //如果 新訂單數量TF009<原訂單數量TF009、新訂單金額TF014<原訂單金額TF114
        //COPCALL1通知主管簽
        COPCALL1 = "N";
        foreach (DataRow od in DT.Rows)
        {
            decimal TF009 = Convert.ToDecimal(od["TF009"].ToString());
            decimal TF109 = Convert.ToDecimal(od["TF109"].ToString());
            decimal TF014 = Convert.ToDecimal(od["TF014"].ToString());
            decimal TF114 = Convert.ToDecimal(od["TF114"].ToString());

            if (TF009< TF109 || TF014< TF114)
            {
                COPCALL1 = "Y";
                break;
            }
            else
            {
                COPCALL1 = "N";
            }
        }

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        COPCHANGEID = SEARCHFORM_VERSION_ID("COP20.訂單變更單");

        if (!string.IsNullOrEmpty(COPCHANGEID))
        {
            Form.SetAttribute("formVersionId", COPCHANGEID);
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
        //TE001NAMES	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE001NAMES");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MQ002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("customValue", "@null");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //CHECKMOC
        //檢查訂單是否有烘培品，DT_CHECK_COPTD_ZINVMBBAKING
        string CHECKMOC = "生產";
        if (DT_CHECK_COPTF_ZINVMBBAKING != null && DT_CHECK_COPTF_ZINVMBBAKING.Rows.Count >= 1)
        {
            CHECKMOC = "烘培";
        }
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CHECKMOC");
        FieldItem.SetAttribute("fieldValue", CHECKMOC);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("customValue", "@null");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE006 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE006"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE001 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE002 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE003 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //COPTCUDF01 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "COPTCUDF01");
        FieldItem.SetAttribute("fieldValue", COPTCUDF01);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //COPCALL1 
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "COPCALL1");
        FieldItem.SetAttribute("fieldValue", COPCALL1);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //10
        //建立節點FieldItem
        //TE038 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE038");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE038"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE007 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE007");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE007"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //MA002 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "MA002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MA002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE011 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE011");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE011"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE111 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE111");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE111"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE012 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE012");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE012"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE112 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE112");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE112"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE041 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE041");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE041"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE041C 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE041C");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE017"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE137 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE137");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE137"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem

        //20
        //TE137C 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE137C");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE117"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE015 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE015");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE015"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE115 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE115");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE115"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE018 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE018");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["NEWTE018"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE118 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE118");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["NEWTE118"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE008 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE008"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //CMSME002A 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CMSME002A");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSME002A"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE108 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE108");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE108"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //CMSME002B 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CMSME002B");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSME002B"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立userset
        var xElement = new XElement(
              new XElement("UserSet",
                  new XElement("Element", new XAttribute("type", "user"),
                      new XElement("userId", BA_USER_GUID)
                      )
                      )
                    );

        //TE009 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE009");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSMV002A"].ToString() + "(" + DT.Rows[0]["TE009"].ToString() + ")");
        FieldItem.SetAttribute("realValue", xElement.ToString());
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //30
        //CMSMV002A 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CMSMV002A");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSMV002A"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE109 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE109");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE109"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //CMSMV002B 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CMSMV002B");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSMV002B"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立userset
        var xElement_BA = new XElement(
              new XElement("UserSet",
                  new XElement("Element", new XAttribute("type", "user"),
                      new XElement("userId", BA_USER_GUID)
                      )
                      )
                    );

        //建立節點FieldItem
        //BA 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "BA");
        FieldItem.SetAttribute("fieldValue", BANAME + "(" + BA + ")");
        FieldItem.SetAttribute("realValue", xElement_BA.ToString());
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //BANAME 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "BANAME");
        FieldItem.SetAttribute("fieldValue", BANAME);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TE040 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE040");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE040"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE136 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE136");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE136"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE013 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE013");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE113 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE113");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE113"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE050 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE050");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE050"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //UDF05
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "UDF05");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["COPTCUDF05"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //MOC 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "MOC");
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
        //PUR 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "PUR");
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
        //DETAILS 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "DETAILS");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點 DataGrid
        XmlElement DataGrid = xmlDoc.CreateElement("DataGrid");
        //DataGrid 加入至 TB 節點底下
        XmlNode DETAILS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='DETAILS']");
        DETAILS.AppendChild(DataGrid);



        foreach (DataRow od in DT.Rows)
        {
            // 新增 Row
            XmlElement Row = xmlDoc.CreateElement("Row");
            Row.SetAttribute("order", (rowscounts).ToString());


            //Row	TF004
            XmlElement Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "COTTDUDF01");
            Cell.SetAttribute("fieldValue", od["COTTDUDF01"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF004
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "COPTFUDF01");
            Cell.SetAttribute("fieldValue", od["COPTFUDF01"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF004
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF004");
            Cell.SetAttribute("fieldValue", od["TF004"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //10
            //Row	TF005
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF005");
            Cell.SetAttribute("fieldValue", od["TF005"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF006
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF006");
            Cell.SetAttribute("fieldValue", od["TF006"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF007
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF007");
            Cell.SetAttribute("fieldValue", od["TF007"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF009
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF009");
            Cell.SetAttribute("fieldValue", od["TF009"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF020
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF020");
            Cell.SetAttribute("fieldValue", od["TF020"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF010
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF010");
            Cell.SetAttribute("fieldValue", od["TF010"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF013
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF013");
            Cell.SetAttribute("fieldValue", od["TF013"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF021
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF021");
            Cell.SetAttribute("fieldValue", od["TF021"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF014
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF014");
            Cell.SetAttribute("fieldValue", od["TF014"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF015
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF015");
            Cell.SetAttribute("fieldValue", od["TF015"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //20
            //Row	TF024
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF024");
            Cell.SetAttribute("fieldValue", od["TF024"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF025
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF025");
            Cell.SetAttribute("fieldValue", od["TF025"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF018
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF018");
            Cell.SetAttribute("fieldValue", od["TF018"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF105
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF105");
            Cell.SetAttribute("fieldValue", od["TF105"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF106
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF106");
            Cell.SetAttribute("fieldValue", od["TF106"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF107
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF107");
            Cell.SetAttribute("fieldValue", od["TF107"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF109
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF109");
            Cell.SetAttribute("fieldValue", od["TF109"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF120
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF120");
            Cell.SetAttribute("fieldValue", od["TF120"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF110
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF110");
            Cell.SetAttribute("fieldValue", od["TF110"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF015
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF113");
            Cell.SetAttribute("fieldValue", od["TF113"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //30
            //Row	TF121
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF121");
            Cell.SetAttribute("fieldValue", od["TF121"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF114
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF114");
            Cell.SetAttribute("fieldValue", od["TF114"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF115
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF115");
            Cell.SetAttribute("fieldValue", od["TF115"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF126
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF126");
            Cell.SetAttribute("fieldValue", od["TF126"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF127
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF127");
            Cell.SetAttribute("fieldValue", od["TF127"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);





            rowscounts = rowscounts + 1;

            XmlNode DataGridS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='DETAILS']/DataGrid");
            DataGridS.AppendChild(Row);

        }

        ////用ADDTACK，直接啟動起單
        ADDTACK(Form);

        //ADD TO DB
        //string connectionString = ConfigurationManager.ConnectionStrings["dbUOF"].ToString();

        //connectionString = ConfigurationManager.ConnectionStrings["dberp"].ConnectionString;
        //sqlConn = new SqlConnection(connectionString);

        ////20210902密
        //Class1 TKID = new Class1();//用new 建立類別實體
        //SqlConnectionStringBuilder sqlsb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["dbUOF"].ConnectionString);

        ////資料庫使用者密碼解密
        //sqlsb.Password = TKID.Decryption(sqlsb.Password);
        //sqlsb.UserID = TKID.Decryption(sqlsb.UserID);

        //String connectionString;
        //sqlConn = new SqlConnection(sqlsb.ConnectionString);
        //connectionString = sqlConn.ConnectionString.ToString();

        //StringBuilder queryString = new StringBuilder();




        //queryString.AppendFormat(@" INSERT INTO [{0}].dbo.TB_WKF_EXTERNAL_TASK
        //                                 (EXTERNAL_TASK_ID,FORM_INFO,STATUS,EXTERNAL_FORM_NBR)
        //                                VALUES (NEWID(),@XML,2,'{1}')
        //                                ", DBNAME, EXTERNAL_FORM_NBR);


        //try
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {

        //        SqlCommand command = new SqlCommand(queryString.ToString(), connection);
        //        command.Parameters.Add("@XML", SqlDbType.NVarChar).Value = Form.OuterXml;

        //        command.Connection.Open();

        //        int count = command.ExecuteNonQuery();

        //        connection.Close();
        //        connection.Dispose();

        //    }
        //}
        //catch
        //{

        //}
        //finally
        //{

        //}
    }

    public void ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF7(string TE001, string TE002, string TE003)
    {

        DataTable DT = SEARCHCOPTECOPTF7(TE001, TE002, TE003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["TE009"].ToString());

        string account = DT.Rows[0]["TE009"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string BA = DT.Rows[0]["BA"].ToString();
        string BANAME = DT.Rows[0]["BANAME"].ToString();
        string BA_USER_GUID = DT.Rows[0]["BA_USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["TE001"].ToString().Trim() + DT.Rows[0]["TE002"].ToString().Trim() + DT.Rows[0]["TE003"].ToString().Trim();

        int rowscounts = 0;

        //COPTFUDF01，如果在變更單要生產的，需簽到生管
        COPTCUDF01 = "N";
        foreach (DataRow od in DT.Rows)
        {
            if (od["COPTFUDF01"].ToString().Equals("Y"))
            {
                COPTCUDF01 = "Y";
                break;
            }
            else
            {
                COPTCUDF01 = "N";
            }
        }

       
        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        COPCHANGEID = SEARCHFORM_VERSION_ID("COP20.訂單變更單");

        if (!string.IsNullOrEmpty(COPCHANGEID))
        {
            Form.SetAttribute("formVersionId", COPCHANGEID);
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
        //CHECKMOC	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CHECKMOC");
        FieldItem.SetAttribute("fieldValue", "生產");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("customValue", "@null");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE006 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE006"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE001 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE002 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE003 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //COPTCUDF01 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "COPTCUDF01");
        FieldItem.SetAttribute("fieldValue", COPTCUDF01);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //COPCALL1 
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "COPCALL1");
        FieldItem.SetAttribute("fieldValue", COPCALL1);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //10
        //建立節點FieldItem
        //TE038 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE038");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE038"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE007 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE007");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE007"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //MA002 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "MA002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MA002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE011 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE011");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE011"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE111 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE111");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE111"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE012 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE012");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE012"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE112 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE112");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE112"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE041 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE041");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE041"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE041C 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE041C");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE017"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem
        //TE137 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE137");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE137"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //建立節點FieldItem

        //20
        //TE137C 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE137C");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE117"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE015 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE015");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE015"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE115 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE115");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE115"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE018 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE018");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["NEWTE018"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE118 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE118");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["NEWTE118"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE008 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE008"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //CMSME002A 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CMSME002A");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSME002A"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE108 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE108");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE108"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //CMSME002B 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CMSME002B");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSME002B"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立userset
        var xElement = new XElement(
              new XElement("UserSet",
                  new XElement("Element", new XAttribute("type", "user"),
                      new XElement("userId", BA_USER_GUID)
                      )
                      )
                    );

        //TE009 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE009");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSMV002A"].ToString() + "(" + DT.Rows[0]["TE009"].ToString() + ")");
        FieldItem.SetAttribute("realValue", xElement.ToString());
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //30
        //CMSMV002A 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CMSMV002A");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSMV002A"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE109 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE109");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE109"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //CMSMV002B 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "CMSMV002B");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["CMSMV002B"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立userset
        var xElement_BA = new XElement(
              new XElement("UserSet",
                  new XElement("Element", new XAttribute("type", "user"),
                      new XElement("userId", BA_USER_GUID)
                      )
                      )
                    );

        //建立節點FieldItem
        //BA 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "BA");
        FieldItem.SetAttribute("fieldValue", BANAME + "(" + BA + ")");
        FieldItem.SetAttribute("realValue", xElement_BA.ToString());
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //BANAME 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "BANAME");
        FieldItem.SetAttribute("fieldValue", BANAME);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TE040 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE040");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE040"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE136 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE136");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE136"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE013 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE013");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE013"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE113 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE113");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE113"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);
        //TE050 	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE050");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE050"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //UDF05
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "UDF05");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["COPTCUDF05"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //MOC 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "MOC");
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
        //PUR 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "PUR");
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
        //DETAILS 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "DETAILS");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點 DataGrid
        XmlElement DataGrid = xmlDoc.CreateElement("DataGrid");
        //DataGrid 加入至 TB 節點底下
        XmlNode DETAILS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='DETAILS']");
        DETAILS.AppendChild(DataGrid);



        foreach (DataRow od in DT.Rows)
        {
            // 新增 Row
            XmlElement Row = xmlDoc.CreateElement("Row");
            Row.SetAttribute("order", (rowscounts).ToString());


            //Row	TF004
            XmlElement Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "COTTDUDF01");
            Cell.SetAttribute("fieldValue", od["COTTDUDF01"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF004
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "COPTFUDF01");
            Cell.SetAttribute("fieldValue", od["COPTFUDF01"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF004
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF004");
            Cell.SetAttribute("fieldValue", od["TF004"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //10
            //Row	TF005
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF005");
            Cell.SetAttribute("fieldValue", od["TF005"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF006
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF006");
            Cell.SetAttribute("fieldValue", od["TF006"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF007
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF007");
            Cell.SetAttribute("fieldValue", od["TF007"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF009
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF009");
            Cell.SetAttribute("fieldValue", od["TF009"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF020
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF020");
            Cell.SetAttribute("fieldValue", od["TF020"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF010
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF010");
            Cell.SetAttribute("fieldValue", od["TF010"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF013
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF013");
            Cell.SetAttribute("fieldValue", od["TF013"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF021
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF021");
            Cell.SetAttribute("fieldValue", od["TF021"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF014
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF014");
            Cell.SetAttribute("fieldValue", od["TF014"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF015
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF015");
            Cell.SetAttribute("fieldValue", od["TF015"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //20
            //Row	TF024
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF024");
            Cell.SetAttribute("fieldValue", od["TF024"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF025
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF025");
            Cell.SetAttribute("fieldValue", od["TF025"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF018
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF018");
            Cell.SetAttribute("fieldValue", od["TF018"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF105
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF105");
            Cell.SetAttribute("fieldValue", od["TF105"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF106
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF106");
            Cell.SetAttribute("fieldValue", od["TF106"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF107
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF107");
            Cell.SetAttribute("fieldValue", od["TF107"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF109
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF109");
            Cell.SetAttribute("fieldValue", od["TF109"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF120
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF120");
            Cell.SetAttribute("fieldValue", od["TF120"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF110
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF110");
            Cell.SetAttribute("fieldValue", od["TF110"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF015
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF113");
            Cell.SetAttribute("fieldValue", od["TF113"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //30
            //Row	TF121
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF121");
            Cell.SetAttribute("fieldValue", od["TF121"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF114
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF114");
            Cell.SetAttribute("fieldValue", od["TF114"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF115
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF115");
            Cell.SetAttribute("fieldValue", od["TF115"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF126
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF126");
            Cell.SetAttribute("fieldValue", od["TF126"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);
            //Row	TF127
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF127");
            Cell.SetAttribute("fieldValue", od["TF127"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            Row.AppendChild(Cell);





            rowscounts = rowscounts + 1;

            XmlNode DataGridS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='DETAILS']/DataGrid");
            DataGridS.AppendChild(Row);

        }

        ////用ADDTACK，直接啟動起單
        ADDTACK(Form);

        //ADD TO DB
        //string connectionString = ConfigurationManager.ConnectionStrings["dbUOF"].ToString();

        //connectionString = ConfigurationManager.ConnectionStrings["dberp"].ConnectionString;
        //sqlConn = new SqlConnection(connectionString);

        ////20210902密
        //Class1 TKID = new Class1();//用new 建立類別實體
        //SqlConnectionStringBuilder sqlsb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["dbUOF"].ConnectionString);

        ////資料庫使用者密碼解密
        //sqlsb.Password = TKID.Decryption(sqlsb.Password);
        //sqlsb.UserID = TKID.Decryption(sqlsb.UserID);

        //String connectionString;
        //sqlConn = new SqlConnection(sqlsb.ConnectionString);
        //connectionString = sqlConn.ConnectionString.ToString();

        //StringBuilder queryString = new StringBuilder();




        //queryString.AppendFormat(@" INSERT INTO [{0}].dbo.TB_WKF_EXTERNAL_TASK
        //                                 (EXTERNAL_TASK_ID,FORM_INFO,STATUS,EXTERNAL_FORM_NBR)
        //                                VALUES (NEWID(),@XML,2,'{1}')
        //                                ", DBNAME, EXTERNAL_FORM_NBR);


        //try
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {

        //        SqlCommand command = new SqlCommand(queryString.ToString(), connection);
        //        command.Parameters.Add("@XML", SqlDbType.NVarChar).Value = Form.OuterXml;

        //        command.Connection.Open();

        //        int count = command.ExecuteNonQuery();

        //        connection.Close();
        //        connection.Dispose();

        //    }
        //}
        //catch
        //{

        //}
        //finally
        //{

        //}
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
            status = "起單成功!";
            formNBR = resultXE.Element("FormNumber").Value;           

            NEWTASK_ID = formNBR;

            Logger.Write("起單", status + formNBR);
            MsgBox("起單成功 \r\n" + TF001 + TF002 +TF003+ " > " + formNBR , this.Page, this);

        }
        else
        {
            status = "起單失敗!";
            error = resultXE.Element("Exception").Element("Message").Value;

            Logger.Write("起單", status + error + "\r\n" + Form.OuterXml);

            MsgBox("失敗了!!!!   " + error + "\r\n" + Form.OuterXml, this.Page, this);

            throw new Exception(status + error + "\r\n" + Form.OuterXml);

        }
    }



    public DataTable SEARCHCOPTECOPTF(string TE001, string TE002, string TE003)
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();

        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();

            cmdTxt.AppendFormat(@"  
                                    SELECT 
                                    ISNULL((SELECT TOP 1 UDF01 FROM  [TK].dbo.COPTD WHERE TD001=TF001 AND TD002=TF002 AND TD003=TF104),'') AS COTTDUDF01
                                    ,COPTFUDF01 AS COPTFUDF01
                                    ,CASE WHEN COPTFUDF01 IN ('N','n','') AND ISNULL((SELECT TOP 1 UDF01 FROM  [TK].dbo.COPTD WHERE TD001=TF001 AND TD002=TF002 AND TD003=TF104),'N') IN ('N','n','') THEN 'N' ELSE 'Y' END AS 'COPTFUDF01CHECK'

                                    ,[COMPANY],[CREATOR],[USR_GROUP],[CREATE_DATE],[MODIFIER],[MODI_DATE],[FLAG],[CREATE_TIME],[MODI_TIME],[TRANS_TYPE],[TRANS_NAME]
                                    ,[sync_date],[sync_time],[sync_mark],[sync_count],[DataUser],[DataGroup]
                                    ,[TE001],[TE002],[TE003],[TE004],[TE005],[TE006],[TE007],[TE008],[TE009],[TE010]
                                    ,[TE011],[TE012],[TE013],[TE014],[TE015],[TE016],[TE017],[TE018],[TE019],[TE020]
                                    ,[TE021],[TE022],[TE023],[TE024],[TE025],[TE026],[TE027],[TE028],[TE029],[TE030]
                                    ,[TE031],[TE032],[TE033],[TE034],[TE035],[TE036],[TE037],[TE038],[TE039],[TE040]
                                    ,[TE041],[TE042],[TE043],[TE044],[TE045],[TE046],[TE047],[TE048],[TE049],[TE050]
                                    ,[TE051],[TE052],[TE053],[TE054],[TE055],[TE056],[TE057],[TE058],[TE059],[TE060]
                                    ,[TE061],[TE062],[TE063],[TE064],[TE065],[TE066],[TE067],[TE068],[TE069],[TE070]
                                    ,[TE071],[TE072],[TE073],[TE074],[TE075],[TE076],[TE077],[TE078],[TE079],[TE080]
                                    ,[TE081],[TE082],[TE083],[TE084],[TE085],[TE086],[TE087],[TE088]
                                    ,[TE103],[TE107],[TE108],[TE109],[TE110]
                                    ,[TE111],[TE112],[TE113],[TE114],[TE115],[TE116],[TE117],[TE118],[TE119],[TE120]
                                    ,[TE121],[TE122],[TE123],[TE124],[TE125],[TE126],[TE127],[TE128],[TE129],[TE130]
                                    ,[TE131],[TE132],[TE133],[TE134],[TE135],[TE136],[TE137],[TE138],[TE139],[TE140]
                                    ,[TE141],[TE142],[TE143],[TE144],[TE145],[TE146],[TE147],[TE148],[TE149],[TE150]
                                    ,[TE151],[TE152],[TE163],[TE164],[TE165],[TE166],[TE167],[TE168],[TE169],[TE170]
                                    ,[TE171],[TE172],[TE173],[TE174],[TE175],[TE176],[TE177],[TE178],[TE179],[TE180]
                                    ,[TE181],[TE182],[TE183],[TE184],[TE185],[TE186],[TE187],[TE188],[TE189],[TE190]
                                    ,[TE191],[TE192],[TE193],[TE194],[TE195],[TE196],[TE197],[TE198],[TE199]
                                    ,[UDF01],[UDF02],[UDF03],[UDF04],[UDF05],[UDF06],[UDF07],[UDF08],[UDF09],[UDF10]
                                    ,[TF001],[TF002],[TF003],[TF004],[TF005],[TF006],[TF007],[TF008],[TF009],[TF010]
                                    ,[TF011],[TF012],[TF013],[TF014],[TF015],[TF016],[TF017],[TF018],[TF019],[TF020]
                                    ,[TF021],[TF022],[TF023],[TF024],[TF025],[TF026],[TF027],[TF028],[TF029],[TF030]
                                    ,[TF031],[TF032],[TF034],[TF035],[TF036],[TF037],[TF038],[TF039],[TF040],[TF041]
                                    ,[TF042],[TF043],[TF044],[TF045],[TF046],[TF048],[TF049],[TF050]
                                    ,[TF051],[TF052],[TF053],[TF054],[TF055],[TF056],[TF057],[TF058],[TF059],[TF060]
                                    ,[TF061],[TF062],[TF063],[TF064],[TF065],[TF066],[TF067],[TF068],[TF069],[TF070]
                                    ,[TF071],[TF072],[TF073],[TF074],[TF075],[TF076],[TF077],[TF078],[TF079],[TF080]
                                    ,[TF104],[TF105],[TF106],[TF107],[TF108],[TF109],[TF110]
                                    ,[TF111],[TF112],[TF113],[TF114],[TF115],[TF116],[TF117],[TF120]
                                    ,[TF121],[TF122],[TF123],[TF124],[TF125],[TF126],[TF127],[TF128],[TF129],[TF130]
                                    ,[TF131],[TF132],[TF133],[TF134],[TF135],[TF136],[TF137],[TF138],[TF139],[TF140]
                                    ,[TF141],[TF142],[TF143],[TF144],[TF145],[TF146],[TF147],[TF148],[TF149],[TF150]
                                    ,[TF151],[TF152],[TF153],[TF154],[TF155],[TF156],[TF157],[TF158],[TF159],[TF160]
                                    ,[TF161],[TF162],[TF163],[TF164],[TF165],[TF166],[TF167],[TF168],[TF169],[TF170]
                                    ,[TF171],[TF172],[TF173],[TF174],[TF175],[TF176],[TF177],[TF178],[TF179],[TF180]
                                    ,[TF181],[TF182],[TF183],[TF184],[TF185],[TF186],[TF187],[TF188],[TF189],[TF190]
                                    ,[TF191],[TF192],[TF193],[TF194],[TF195],[TF196],[TF197],[TF198],[TF199]
                                    ,[TF200],[TF300]
                                    ,COPTFUDF01,COPTFUDF02,COPTFUDF03,COPTFUDF04,COPTFUDF05,COPTFUDF06,COPTFUDF07,COPTFUDF08,COPTFUDF09,COPTFUDF10
                                    ,USER_GUID,NAME
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'
                                    ,MA002
                                    ,CASE WHEN TE018='1' THEN '1.應稅內含'  ELSE (CASE WHEN TE018='2' THEN '2.應稅外加'  ELSE (CASE WHEN TE018='3' THEN '3.零稅率'  ELSE (CASE WHEN TE018='4' THEN '4.免稅'  ELSE (CASE WHEN TE018='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTE018'
                                    ,CASE WHEN TE118='1' THEN '1.應稅內含'  ELSE (CASE WHEN TE118='2' THEN '2.應稅外加'  ELSE (CASE WHEN TE118='3' THEN '3.零稅率'  ELSE (CASE WHEN TE118='4' THEN '4.免稅'  ELSE (CASE WHEN TE118='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTE118'
                                    ,(SELECT TOP 1 ME002 FROM [TK].dbo.CMSME WHERE ME001=TE008) AS 'CMSME002A'
                                    ,(SELECT TOP 1 ME002 FROM [TK].dbo.CMSME WHERE ME001=TE108) AS 'CMSME002B'
                                    ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE009) AS 'CMSMV002A'
                                    ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE109) AS 'CMSMV002B'
                                    ,(SELECT TOP 1 COPTC.UDF05 FROM [TK].dbo.COPTC WHERE TC001=TE001 AND TC002=TE002) AS 'COPTCUDF05'
                                    
                                    ,BA
                                    ,BANAME
                                    ,(SELECT TOP 1 [USER_GUID] FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER] WHERE [ACCOUNT]=BA COLLATE Chinese_Taiwan_Stroke_BIN) AS 'BA_USER_GUID'
                                    ,MQ002

                                    FROM 
                                    (
                                    SELECT 
                                    [COPTE].[COMPANY],[COPTE].[CREATOR],[COPTE].[USR_GROUP],[COPTE].[CREATE_DATE],[COPTE].[MODIFIER],[COPTE].[MODI_DATE],[COPTE].[FLAG],[COPTE].[CREATE_TIME],[COPTE].[MODI_TIME],[COPTE].[TRANS_TYPE],[COPTE].[TRANS_NAME]
                                    ,[COPTE].[sync_date],[COPTE].[sync_time],[COPTE].[sync_mark],[COPTE].[sync_count],[COPTE].[DataUser],[COPTE].[DataGroup]
                                    ,[COPTE].[TE001],[COPTE].[TE002],[COPTE].[TE003],[COPTE].[TE004],[COPTE].[TE005],[COPTE].[TE006],[COPTE].[TE007],[COPTE].[TE008],[COPTE].[TE009],[COPTE].[TE010]
                                    ,[COPTE].[TE011],[COPTE].[TE012],[COPTE].[TE013],[COPTE].[TE014],[COPTE].[TE015],[COPTE].[TE016],[COPTE].[TE017],[COPTE].[TE018],[COPTE].[TE019],[COPTE].[TE020]
                                    ,[COPTE].[TE021],[COPTE].[TE022],[COPTE].[TE023],[COPTE].[TE024],[COPTE].[TE025],[COPTE].[TE026],[COPTE].[TE027],[COPTE].[TE028],[COPTE].[TE029],[COPTE].[TE030]
                                    ,[COPTE].[TE031],[COPTE].[TE032],[COPTE].[TE033],[COPTE].[TE034],[COPTE].[TE035],[COPTE].[TE036],[COPTE].[TE037],[COPTE].[TE038],[COPTE].[TE039],[COPTE].[TE040]
                                    ,[COPTE].[TE041],[COPTE].[TE042],[COPTE].[TE043],[COPTE].[TE044],[COPTE].[TE045],[COPTE].[TE046],[COPTE].[TE047],[COPTE].[TE048],[COPTE].[TE049],[COPTE].[TE050]
                                    ,[COPTE].[TE051],[COPTE].[TE052],[COPTE].[TE053],[COPTE].[TE054],[COPTE].[TE055],[COPTE].[TE056],[COPTE].[TE057],[COPTE].[TE058],[COPTE].[TE059],[COPTE].[TE060]
                                    ,[COPTE].[TE061],[COPTE].[TE062],[COPTE].[TE063],[COPTE].[TE064],[COPTE].[TE065],[COPTE].[TE066],[COPTE].[TE067],[COPTE].[TE068],[COPTE].[TE069],[COPTE].[TE070]
                                    ,[COPTE].[TE071],[COPTE].[TE072],[COPTE].[TE073],[COPTE].[TE074],[COPTE].[TE075],[COPTE].[TE076],[COPTE].[TE077],[COPTE].[TE078],[COPTE].[TE079],[COPTE].[TE080]
                                    ,[COPTE].[TE081],[COPTE].[TE082],[COPTE].[TE083],[COPTE].[TE084],[COPTE].[TE085],[COPTE].[TE086],[COPTE].[TE087],[COPTE].[TE088]
                                    ,[COPTE].[TE103],[COPTE].[TE107],[COPTE].[TE108],[COPTE].[TE109],[COPTE].[TE110]
                                    ,[COPTE].[TE111],[COPTE].[TE112],[COPTE].[TE113],[COPTE].[TE114],[COPTE].[TE115],[COPTE].[TE116],[COPTE].[TE117],[COPTE].[TE118],[COPTE].[TE119],[COPTE].[TE120]
                                    ,[COPTE].[TE121],[COPTE].[TE122],[COPTE].[TE123],[COPTE].[TE124],[COPTE].[TE125],[COPTE].[TE126],[COPTE].[TE127],[COPTE].[TE128],[COPTE].[TE129],[COPTE].[TE130]
                                    ,[COPTE].[TE131],[COPTE].[TE132],[COPTE].[TE133],[COPTE].[TE134],[COPTE].[TE135],[COPTE].[TE136],[COPTE].[TE137],[COPTE].[TE138],[COPTE].[TE139],[COPTE].[TE140]
                                    ,[COPTE].[TE141],[COPTE].[TE142],[COPTE].[TE143],[COPTE].[TE144],[COPTE].[TE145],[COPTE].[TE146],[COPTE].[TE147],[COPTE].[TE148],[COPTE].[TE149],[COPTE].[TE150]
                                    ,[COPTE].[TE151],[COPTE].[TE152],[COPTE].[TE163],[COPTE].[TE164],[COPTE].[TE165],[COPTE].[TE166],[COPTE].[TE167],[COPTE].[TE168],[COPTE].[TE169],[COPTE].[TE170]
                                    ,[COPTE].[TE171],[COPTE].[TE172],[COPTE].[TE173],[COPTE].[TE174],[COPTE].[TE175],[COPTE].[TE176],[COPTE].[TE177],[COPTE].[TE178],[COPTE].[TE179],[COPTE].[TE180]
                                    ,[COPTE].[TE181],[COPTE].[TE182],[COPTE].[TE183],[COPTE].[TE184],[COPTE].[TE185],[COPTE].[TE186],[COPTE].[TE187],[COPTE].[TE188],[COPTE].[TE189],[COPTE].[TE190]
                                    ,[COPTE].[TE191],[COPTE].[TE192],[COPTE].[TE193],[COPTE].[TE194],[COPTE].[TE195],[COPTE].[TE196],[COPTE].[TE197],[COPTE].[TE198],[COPTE].[TE199]
                                    ,[COPTE].[UDF01],[COPTE].[UDF02],[COPTE].[UDF03],[COPTE].[UDF04],[COPTE].[UDF05],[COPTE].[UDF06],[COPTE].[UDF07],[COPTE].[UDF08],[COPTE].[UDF09],[COPTE].[UDF10]
                                    ,[COPTF].[TF001],[COPTF].[TF002],[COPTF].[TF003],[COPTF].[TF004],[COPTF].[TF005],[COPTF].[TF006],[COPTF].[TF007],[COPTF].[TF008],[COPTF].[TF009],[COPTF].[TF010]
                                    ,[COPTF].[TF011],[COPTF].[TF012],[COPTF].[TF013],[COPTF].[TF014],[COPTF].[TF015],[COPTF].[TF016],[COPTF].[TF017],[COPTF].[TF018],[COPTF].[TF019],[COPTF].[TF020]
                                    ,[COPTF].[TF021],[COPTF].[TF022],[COPTF].[TF023],[COPTF].[TF024],[COPTF].[TF025],[COPTF].[TF026],[COPTF].[TF027],[COPTF].[TF028],[COPTF].[TF029],[COPTF].[TF030]
                                    ,[COPTF].[TF031],[COPTF].[TF032],[COPTF].[TF034],[COPTF].[TF035],[COPTF].[TF036],[COPTF].[TF037],[COPTF].[TF038],[COPTF].[TF039],[COPTF].[TF040],[COPTF].[TF041]
                                    ,[COPTF].[TF042],[COPTF].[TF043],[COPTF].[TF044],[COPTF].[TF045],[COPTF].[TF046],[COPTF].[TF048],[COPTF].[TF049],[COPTF].[TF050]
                                    ,[COPTF].[TF051],[COPTF].[TF052],[COPTF].[TF053],[COPTF].[TF054],[COPTF].[TF055],[COPTF].[TF056],[COPTF].[TF057],[COPTF].[TF058],[COPTF].[TF059],[COPTF].[TF060]
                                    ,[COPTF].[TF061],[COPTF].[TF062],[COPTF].[TF063],[COPTF].[TF064],[COPTF].[TF065],[COPTF].[TF066],[COPTF].[TF067],[COPTF].[TF068],[COPTF].[TF069],[COPTF].[TF070]
                                    ,[COPTF].[TF071],[COPTF].[TF072],[COPTF].[TF073],[COPTF].[TF074],[COPTF].[TF075],[COPTF].[TF076],[COPTF].[TF077],[COPTF].[TF078],[COPTF].[TF079],[COPTF].[TF080]
                                    ,[COPTF].[TF104],[COPTF].[TF105],[COPTF].[TF106],[COPTF].[TF107],[COPTF].[TF108],[COPTF].[TF109],[COPTF].[TF110]
                                    ,[COPTF].[TF111],[COPTF].[TF112],[COPTF].[TF113],[COPTF].[TF114],[COPTF].[TF115],[COPTF].[TF116],[COPTF].[TF117],[COPTF].[TF120]
                                    ,[COPTF].[TF121],[COPTF].[TF122],[COPTF].[TF123],[COPTF].[TF124],[COPTF].[TF125],[COPTF].[TF126],[COPTF].[TF127],[COPTF].[TF128],[COPTF].[TF129],[COPTF].[TF130]
                                    ,[COPTF].[TF131],[COPTF].[TF132],[COPTF].[TF133],[COPTF].[TF134],[COPTF].[TF135],[COPTF].[TF136],[COPTF].[TF137],[COPTF].[TF138],[COPTF].[TF139],[COPTF].[TF140]
                                    ,[COPTF].[TF141],[COPTF].[TF142],[COPTF].[TF143],[COPTF].[TF144],[COPTF].[TF145],[COPTF].[TF146],[COPTF].[TF147],[COPTF].[TF148],[COPTF].[TF149],[COPTF].[TF150]
                                    ,[COPTF].[TF151],[COPTF].[TF152],[COPTF].[TF153],[COPTF].[TF154],[COPTF].[TF155],[COPTF].[TF156],[COPTF].[TF157],[COPTF].[TF158],[COPTF].[TF159],[COPTF].[TF160]
                                    ,[COPTF].[TF161],[COPTF].[TF162],[COPTF].[TF163],[COPTF].[TF164],[COPTF].[TF165],[COPTF].[TF166],[COPTF].[TF167],[COPTF].[TF168],[COPTF].[TF169],[COPTF].[TF170]
                                    ,[COPTF].[TF171],[COPTF].[TF172],[COPTF].[TF173],[COPTF].[TF174],[COPTF].[TF175],[COPTF].[TF176],[COPTF].[TF177],[COPTF].[TF178],[COPTF].[TF179],[COPTF].[TF180]
                                    ,[COPTF].[TF181],[COPTF].[TF182],[COPTF].[TF183],[COPTF].[TF184],[COPTF].[TF185],[COPTF].[TF186],[COPTF].[TF187],[COPTF].[TF188],[COPTF].[TF189],[COPTF].[TF190]
                                    ,[COPTF].[TF191],[COPTF].[TF192],[COPTF].[TF193],[COPTF].[TF194],[COPTF].[TF195],[COPTF].[TF196],[COPTF].[TF197],[COPTF].[TF198],[COPTF].[TF199]
                                    ,[COPTF].[TF200],[COPTF].[TF300]
                                    ,[COPTF].[UDF01] AS 'COPTFUDF01',[COPTF].[UDF02] AS 'COPTFUDF02',[COPTF].[UDF03] AS 'COPTFUDF03',[COPTF].[UDF04] AS 'COPTFUDF04',[COPTF].[UDF05] AS 'COPTFUDF05',[COPTF].[UDF06] AS 'COPTFUDF06',[COPTF].[UDF07] AS 'COPTFUDF07',[COPTF].[UDF08] AS 'COPTFUDF08',[COPTF].[UDF09] AS 'COPTFUDF09',[COPTF].[UDF10] AS 'COPTFUDF10'
                                    ,[TB_EB_USER].USER_GUID,NAME
                                    ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE009) AS 'MV002'
                                    ,(SELECT TOP 1 MA002 FROM [TK].dbo.COPMA WHERE MA001=TE007) AS 'MA002'
                                    ,(SELECT TOP 1 COPMA.UDF04 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TE007) AS 'BA'
                                    ,(SELECT TOP 1 CMSMV.MV002 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TE007) AS 'BANAME'
                                    ,MQ002

                                    FROM [TK].dbo.COPTF,[TK].dbo.COPTE
                                    LEFT JOIN [192.168.1.223].[{0}].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TE009 COLLATE Chinese_Taiwan_Stroke_BIN
                                    LEFT JOIN [TK].dbo.CMSMQ ON TE001=MQ001

                                    WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                    AND TE001='{1}' AND TE002='{2}' AND TE003='{3}'
                                    ) AS TEMP   
                              
                                    ", DBNAME, TE001, TE002, TE003);


            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



            if (dt.Rows.Count >= 1)
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
        {
           
        }
    }

    public DataTable SEARCHCOPTECOPTF7(string TE001, string TE002, string TE003)
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();

        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();

            cmdTxt.AppendFormat(@"  
                                    SELECT 
                                    ISNULL((SELECT TOP 1 UDF01 FROM  [TK].dbo.COPTD WHERE TD001=TF001 AND TD002=TF002 AND TD003=TF104),'') AS COTTDUDF01
                                    ,COPTFUDF01 AS COPTFUDF01
                                    ,CASE WHEN COPTFUDF01 IN ('N','n','') AND ISNULL((SELECT TOP 1 UDF01 FROM  [TK].dbo.COPTD WHERE TD001=TF001 AND TD002=TF002 AND TD003=TF104),'N') IN ('N','n','') THEN 'N' ELSE 'Y' END AS 'COPTFUDF01CHECK'

                                    ,[COMPANY],[CREATOR],[USR_GROUP],[CREATE_DATE],[MODIFIER],[MODI_DATE],[FLAG],[CREATE_TIME],[MODI_TIME],[TRANS_TYPE],[TRANS_NAME]
                                    ,[sync_date],[sync_time],[sync_mark],[sync_count],[DataUser],[DataGroup]
                                    ,[TE001],[TE002],[TE003],[TE004],[TE005],[TE006],[TE007],[TE008],[TE009],[TE010]
                                    ,[TE011],[TE012],[TE013],[TE014],[TE015],[TE016],[TE017],[TE018],[TE019],[TE020]
                                    ,[TE021],[TE022],[TE023],[TE024],[TE025],[TE026],[TE027],[TE028],[TE029],[TE030]
                                    ,[TE031],[TE032],[TE033],[TE034],[TE035],[TE036],[TE037],[TE038],[TE039],[TE040]
                                    ,[TE041],[TE042],[TE043],[TE044],[TE045],[TE046],[TE047],[TE048],[TE049],[TE050]
                                    ,[TE051],[TE052],[TE053],[TE054],[TE055],[TE056],[TE057],[TE058],[TE059],[TE060]
                                    ,[TE061],[TE062],[TE063],[TE064],[TE065],[TE066],[TE067],[TE068],[TE069],[TE070]
                                    ,[TE071],[TE072],[TE073],[TE074],[TE075],[TE076],[TE077],[TE078],[TE079],[TE080]
                                    ,[TE081],[TE082],[TE083],[TE084],[TE085],[TE086],[TE087],[TE088]
                                    ,[TE103],[TE107],[TE108],[TE109],[TE110]
                                    ,[TE111],[TE112],[TE113],[TE114],[TE115],[TE116],[TE117],[TE118],[TE119],[TE120]
                                    ,[TE121],[TE122],[TE123],[TE124],[TE125],[TE126],[TE127],[TE128],[TE129],[TE130]
                                    ,[TE131],[TE132],[TE133],[TE134],[TE135],[TE136],[TE137],[TE138],[TE139],[TE140]
                                    ,[TE141],[TE142],[TE143],[TE144],[TE145],[TE146],[TE147],[TE148],[TE149],[TE150]
                                    ,[TE151],[TE152],[TE163],[TE164],[TE165],[TE166],[TE167],[TE168],[TE169],[TE170]
                                    ,[TE171],[TE172],[TE173],[TE174],[TE175],[TE176],[TE177],[TE178],[TE179],[TE180]
                                    ,[TE181],[TE182],[TE183],[TE184],[TE185],[TE186],[TE187],[TE188],[TE189],[TE190]
                                    ,[TE191],[TE192],[TE193],[TE194],[TE195],[TE196],[TE197],[TE198],[TE199]
                                    ,[UDF01],[UDF02],[UDF03],[UDF04],[UDF05],[UDF06],[UDF07],[UDF08],[UDF09],[UDF10]
                                    ,[TF001],[TF002],[TF003],[TF004],[TF005],[TF006],[TF007],[TF008],[TF009],[TF010]
                                    ,[TF011],[TF012],[TF013],[TF014],[TF015],[TF016],[TF017],[TF018],[TF019],[TF020]
                                    ,[TF021],[TF022],[TF023],[TF024],[TF025],[TF026],[TF027],[TF028],[TF029],[TF030]
                                    ,[TF031],[TF032],[TF034],[TF035],[TF036],[TF037],[TF038],[TF039],[TF040],[TF041]
                                    ,[TF042],[TF043],[TF044],[TF045],[TF046],[TF048],[TF049],[TF050]
                                    ,[TF051],[TF052],[TF053],[TF054],[TF055],[TF056],[TF057],[TF058],[TF059],[TF060]
                                    ,[TF061],[TF062],[TF063],[TF064],[TF065],[TF066],[TF067],[TF068],[TF069],[TF070]
                                    ,[TF071],[TF072],[TF073],[TF074],[TF075],[TF076],[TF077],[TF078],[TF079],[TF080]
                                    ,[TF104],[TF105],[TF106],[TF107],[TF108],[TF109],[TF110]
                                    ,[TF111],[TF112],[TF113],[TF114],[TF115],[TF116],[TF117],[TF120]
                                    ,[TF121],[TF122],[TF123],[TF124],[TF125],[TF126],[TF127],[TF128],[TF129],[TF130]
                                    ,[TF131],[TF132],[TF133],[TF134],[TF135],[TF136],[TF137],[TF138],[TF139],[TF140]
                                    ,[TF141],[TF142],[TF143],[TF144],[TF145],[TF146],[TF147],[TF148],[TF149],[TF150]
                                    ,[TF151],[TF152],[TF153],[TF154],[TF155],[TF156],[TF157],[TF158],[TF159],[TF160]
                                    ,[TF161],[TF162],[TF163],[TF164],[TF165],[TF166],[TF167],[TF168],[TF169],[TF170]
                                    ,[TF171],[TF172],[TF173],[TF174],[TF175],[TF176],[TF177],[TF178],[TF179],[TF180]
                                    ,[TF181],[TF182],[TF183],[TF184],[TF185],[TF186],[TF187],[TF188],[TF189],[TF190]
                                    ,[TF191],[TF192],[TF193],[TF194],[TF195],[TF196],[TF197],[TF198],[TF199]
                                    ,[TF200],[TF300]
                                    ,COPTFUDF01,COPTFUDF02,COPTFUDF03,COPTFUDF04,COPTFUDF05,COPTFUDF06,COPTFUDF07,COPTFUDF08,COPTFUDF09,COPTFUDF10
                                    ,USER_GUID,NAME
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'
                                    ,MA002
                                    ,CASE WHEN TE018='1' THEN '1.應稅內含'  ELSE (CASE WHEN TE018='2' THEN '2.應稅外加'  ELSE (CASE WHEN TE018='3' THEN '3.零稅率'  ELSE (CASE WHEN TE018='4' THEN '4.免稅'  ELSE (CASE WHEN TE018='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTE018'
                                    ,CASE WHEN TE118='1' THEN '1.應稅內含'  ELSE (CASE WHEN TE118='2' THEN '2.應稅外加'  ELSE (CASE WHEN TE118='3' THEN '3.零稅率'  ELSE (CASE WHEN TE118='4' THEN '4.免稅'  ELSE (CASE WHEN TE118='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTE118'
                                    ,(SELECT TOP 1 ME002 FROM [TK].dbo.CMSME WHERE ME001=TE008) AS 'CMSME002A'
                                    ,(SELECT TOP 1 ME002 FROM [TK].dbo.CMSME WHERE ME001=TE108) AS 'CMSME002B'
                                    ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE009) AS 'CMSMV002A'
                                    ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE109) AS 'CMSMV002B'
                                    ,(SELECT TOP 1 COPTC.UDF05 FROM [TK].dbo.COPTC WHERE TC001=TE001 AND TC002=TE002) AS 'COPTCUDF05'
                                    
                                    ,BA
                                    ,BANAME
                                    ,(SELECT TOP 1 [USER_GUID] FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER] WHERE [ACCOUNT]=BA COLLATE Chinese_Taiwan_Stroke_BIN) AS 'BA_USER_GUID'
                                    FROM 
                                    (
                                    SELECT 
                                    [COPTE].[COMPANY],[COPTE].[CREATOR],[COPTE].[USR_GROUP],[COPTE].[CREATE_DATE],[COPTE].[MODIFIER],[COPTE].[MODI_DATE],[COPTE].[FLAG],[COPTE].[CREATE_TIME],[COPTE].[MODI_TIME],[COPTE].[TRANS_TYPE],[COPTE].[TRANS_NAME]
                                    ,[COPTE].[sync_date],[COPTE].[sync_time],[COPTE].[sync_mark],[COPTE].[sync_count],[COPTE].[DataUser],[COPTE].[DataGroup]
                                    ,[COPTE].[TE001],[COPTE].[TE002],[COPTE].[TE003],[COPTE].[TE004],[COPTE].[TE005],[COPTE].[TE006],[COPTE].[TE007],[COPTE].[TE008],[COPTE].[TE009],[COPTE].[TE010]
                                    ,[COPTE].[TE011],[COPTE].[TE012],[COPTE].[TE013],[COPTE].[TE014],[COPTE].[TE015],[COPTE].[TE016],[COPTE].[TE017],[COPTE].[TE018],[COPTE].[TE019],[COPTE].[TE020]
                                    ,[COPTE].[TE021],[COPTE].[TE022],[COPTE].[TE023],[COPTE].[TE024],[COPTE].[TE025],[COPTE].[TE026],[COPTE].[TE027],[COPTE].[TE028],[COPTE].[TE029],[COPTE].[TE030]
                                    ,[COPTE].[TE031],[COPTE].[TE032],[COPTE].[TE033],[COPTE].[TE034],[COPTE].[TE035],[COPTE].[TE036],[COPTE].[TE037],[COPTE].[TE038],[COPTE].[TE039],[COPTE].[TE040]
                                    ,[COPTE].[TE041],[COPTE].[TE042],[COPTE].[TE043],[COPTE].[TE044],[COPTE].[TE045],[COPTE].[TE046],[COPTE].[TE047],[COPTE].[TE048],[COPTE].[TE049],[COPTE].[TE050]
                                    ,[COPTE].[TE051],[COPTE].[TE052],[COPTE].[TE053],[COPTE].[TE054],[COPTE].[TE055],[COPTE].[TE056],[COPTE].[TE057],[COPTE].[TE058],[COPTE].[TE059],[COPTE].[TE060]
                                    ,[COPTE].[TE061],[COPTE].[TE062],[COPTE].[TE063],[COPTE].[TE064],[COPTE].[TE065],[COPTE].[TE066],[COPTE].[TE067],[COPTE].[TE068],[COPTE].[TE069],[COPTE].[TE070]
                                    ,[COPTE].[TE071],[COPTE].[TE072],[COPTE].[TE073],[COPTE].[TE074],[COPTE].[TE075],[COPTE].[TE076],[COPTE].[TE077],[COPTE].[TE078],[COPTE].[TE079],[COPTE].[TE080]
                                    ,[COPTE].[TE081],[COPTE].[TE082],[COPTE].[TE083],[COPTE].[TE084],[COPTE].[TE085],[COPTE].[TE086],[COPTE].[TE087],[COPTE].[TE088]
                                    ,[COPTE].[TE103],[COPTE].[TE107],[COPTE].[TE108],[COPTE].[TE109],[COPTE].[TE110]
                                    ,[COPTE].[TE111],[COPTE].[TE112],[COPTE].[TE113],[COPTE].[TE114],[COPTE].[TE115],[COPTE].[TE116],[COPTE].[TE117],[COPTE].[TE118],[COPTE].[TE119],[COPTE].[TE120]
                                    ,[COPTE].[TE121],[COPTE].[TE122],[COPTE].[TE123],[COPTE].[TE124],[COPTE].[TE125],[COPTE].[TE126],[COPTE].[TE127],[COPTE].[TE128],[COPTE].[TE129],[COPTE].[TE130]
                                    ,[COPTE].[TE131],[COPTE].[TE132],[COPTE].[TE133],[COPTE].[TE134],[COPTE].[TE135],[COPTE].[TE136],[COPTE].[TE137],[COPTE].[TE138],[COPTE].[TE139],[COPTE].[TE140]
                                    ,[COPTE].[TE141],[COPTE].[TE142],[COPTE].[TE143],[COPTE].[TE144],[COPTE].[TE145],[COPTE].[TE146],[COPTE].[TE147],[COPTE].[TE148],[COPTE].[TE149],[COPTE].[TE150]
                                    ,[COPTE].[TE151],[COPTE].[TE152],[COPTE].[TE163],[COPTE].[TE164],[COPTE].[TE165],[COPTE].[TE166],[COPTE].[TE167],[COPTE].[TE168],[COPTE].[TE169],[COPTE].[TE170]
                                    ,[COPTE].[TE171],[COPTE].[TE172],[COPTE].[TE173],[COPTE].[TE174],[COPTE].[TE175],[COPTE].[TE176],[COPTE].[TE177],[COPTE].[TE178],[COPTE].[TE179],[COPTE].[TE180]
                                    ,[COPTE].[TE181],[COPTE].[TE182],[COPTE].[TE183],[COPTE].[TE184],[COPTE].[TE185],[COPTE].[TE186],[COPTE].[TE187],[COPTE].[TE188],[COPTE].[TE189],[COPTE].[TE190]
                                    ,[COPTE].[TE191],[COPTE].[TE192],[COPTE].[TE193],[COPTE].[TE194],[COPTE].[TE195],[COPTE].[TE196],[COPTE].[TE197],[COPTE].[TE198],[COPTE].[TE199]
                                    ,[COPTE].[UDF01],[COPTE].[UDF02],[COPTE].[UDF03],[COPTE].[UDF04],[COPTE].[UDF05],[COPTE].[UDF06],[COPTE].[UDF07],[COPTE].[UDF08],[COPTE].[UDF09],[COPTE].[UDF10]
                                    ,[COPTF].[TF001],[COPTF].[TF002],[COPTF].[TF003],[COPTF].[TF004],[COPTF].[TF005],[COPTF].[TF006],[COPTF].[TF007],[COPTF].[TF008],[COPTF].[TF009],[COPTF].[TF010]
                                    ,[COPTF].[TF011],[COPTF].[TF012],[COPTF].[TF013],[COPTF].[TF014],[COPTF].[TF015],[COPTF].[TF016],[COPTF].[TF017],[COPTF].[TF018],[COPTF].[TF019],[COPTF].[TF020]
                                    ,[COPTF].[TF021],[COPTF].[TF022],[COPTF].[TF023],[COPTF].[TF024],[COPTF].[TF025],[COPTF].[TF026],[COPTF].[TF027],[COPTF].[TF028],[COPTF].[TF029],[COPTF].[TF030]
                                    ,[COPTF].[TF031],[COPTF].[TF032],[COPTF].[TF034],[COPTF].[TF035],[COPTF].[TF036],[COPTF].[TF037],[COPTF].[TF038],[COPTF].[TF039],[COPTF].[TF040],[COPTF].[TF041]
                                    ,[COPTF].[TF042],[COPTF].[TF043],[COPTF].[TF044],[COPTF].[TF045],[COPTF].[TF046],[COPTF].[TF048],[COPTF].[TF049],[COPTF].[TF050]
                                    ,[COPTF].[TF051],[COPTF].[TF052],[COPTF].[TF053],[COPTF].[TF054],[COPTF].[TF055],[COPTF].[TF056],[COPTF].[TF057],[COPTF].[TF058],[COPTF].[TF059],[COPTF].[TF060]
                                    ,[COPTF].[TF061],[COPTF].[TF062],[COPTF].[TF063],[COPTF].[TF064],[COPTF].[TF065],[COPTF].[TF066],[COPTF].[TF067],[COPTF].[TF068],[COPTF].[TF069],[COPTF].[TF070]
                                    ,[COPTF].[TF071],[COPTF].[TF072],[COPTF].[TF073],[COPTF].[TF074],[COPTF].[TF075],[COPTF].[TF076],[COPTF].[TF077],[COPTF].[TF078],[COPTF].[TF079],[COPTF].[TF080]
                                    ,[COPTF].[TF104],[COPTF].[TF105],[COPTF].[TF106],[COPTF].[TF107],[COPTF].[TF108],[COPTF].[TF109],[COPTF].[TF110]
                                    ,[COPTF].[TF111],[COPTF].[TF112],[COPTF].[TF113],[COPTF].[TF114],[COPTF].[TF115],[COPTF].[TF116],[COPTF].[TF117],[COPTF].[TF120]
                                    ,[COPTF].[TF121],[COPTF].[TF122],[COPTF].[TF123],[COPTF].[TF124],[COPTF].[TF125],[COPTF].[TF126],[COPTF].[TF127],[COPTF].[TF128],[COPTF].[TF129],[COPTF].[TF130]
                                    ,[COPTF].[TF131],[COPTF].[TF132],[COPTF].[TF133],[COPTF].[TF134],[COPTF].[TF135],[COPTF].[TF136],[COPTF].[TF137],[COPTF].[TF138],[COPTF].[TF139],[COPTF].[TF140]
                                    ,[COPTF].[TF141],[COPTF].[TF142],[COPTF].[TF143],[COPTF].[TF144],[COPTF].[TF145],[COPTF].[TF146],[COPTF].[TF147],[COPTF].[TF148],[COPTF].[TF149],[COPTF].[TF150]
                                    ,[COPTF].[TF151],[COPTF].[TF152],[COPTF].[TF153],[COPTF].[TF154],[COPTF].[TF155],[COPTF].[TF156],[COPTF].[TF157],[COPTF].[TF158],[COPTF].[TF159],[COPTF].[TF160]
                                    ,[COPTF].[TF161],[COPTF].[TF162],[COPTF].[TF163],[COPTF].[TF164],[COPTF].[TF165],[COPTF].[TF166],[COPTF].[TF167],[COPTF].[TF168],[COPTF].[TF169],[COPTF].[TF170]
                                    ,[COPTF].[TF171],[COPTF].[TF172],[COPTF].[TF173],[COPTF].[TF174],[COPTF].[TF175],[COPTF].[TF176],[COPTF].[TF177],[COPTF].[TF178],[COPTF].[TF179],[COPTF].[TF180]
                                    ,[COPTF].[TF181],[COPTF].[TF182],[COPTF].[TF183],[COPTF].[TF184],[COPTF].[TF185],[COPTF].[TF186],[COPTF].[TF187],[COPTF].[TF188],[COPTF].[TF189],[COPTF].[TF190]
                                    ,[COPTF].[TF191],[COPTF].[TF192],[COPTF].[TF193],[COPTF].[TF194],[COPTF].[TF195],[COPTF].[TF196],[COPTF].[TF197],[COPTF].[TF198],[COPTF].[TF199]
                                    ,[COPTF].[TF200],[COPTF].[TF300]
                                    ,[COPTF].[UDF01] AS 'COPTFUDF01',[COPTF].[UDF02] AS 'COPTFUDF02',[COPTF].[UDF03] AS 'COPTFUDF03',[COPTF].[UDF04] AS 'COPTFUDF04',[COPTF].[UDF05] AS 'COPTFUDF05',[COPTF].[UDF06] AS 'COPTFUDF06',[COPTF].[UDF07] AS 'COPTFUDF07',[COPTF].[UDF08] AS 'COPTFUDF08',[COPTF].[UDF09] AS 'COPTFUDF09',[COPTF].[UDF10] AS 'COPTFUDF10'
                                    ,[TB_EB_USER].USER_GUID,NAME
                                    ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE009) AS 'MV002'
                                    ,(SELECT TOP 1 MA002 FROM [TK].dbo.COPMA WHERE MA001=TE007) AS 'MA002'
                                    ,(SELECT TOP 1 COPMA.UDF04 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TE007) AS 'BA'
                                    ,(SELECT TOP 1 CMSMV.MV002 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TE007) AS 'BANAME'
                                    FROM [TK].dbo.COPTE
                                    LEFT JOIN [TK].dbo.COPTF ON TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                    LEFT JOIN [192.168.1.223].[{0}].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TE009 COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE 1=1
                                    AND TE001='{1}' AND TE002='{2}' AND TE003='{3}'
                                    ) AS TEMP   
                              
                                    ", DBNAME, TE001, TE002, TE003);


            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



            if (dt.Rows.Count >= 1)
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
        {

        }
    }


    public DataTable SEARCHUOFDEP(string ACCOUNT)
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();

        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();



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
                                FROM [192.168.1.223].[{0}].[dbo].[TB_EB_USER],[192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP],[192.168.1.223].[{0}].[dbo].[TB_EB_GROUP]
                                WHERE [TB_EB_USER].[USER_GUID]=[TB_EB_EMPL_DEP].[USER_GUID]
                                AND [TB_EB_EMPL_DEP].[GROUP_ID]=[TB_EB_GROUP].[GROUP_ID]
                                AND ISNULL([TB_EB_GROUP].[GROUP_CODE],'')<>''
                                AND [ACCOUNT]='{1}'
                              
                                ", DBNAME, ACCOUNT);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));
           

            if (dt.Rows.Count >= 1)
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
        {
           
        }
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

    public void ADDMOC()
    {
        string TF001 = null;
        string TF002 = null;
        string TF003 = null;
        string TF004 = null;
        string MOCCHECKSCOMMENTS = null;
        string MOCCHECKS = null;

        foreach (GridViewRow gvr in this.Grid2.Rows)
        {           
            var GRIDVIEWTextBox1 = (TextBox)gvr.FindControl("GRIDVIEWTextBox1");
            var GRIDVIEWDropDownList1 = (DropDownList)gvr.FindControl("GRIDVIEWDropDownList1");

            TableCellCollection cell = gvr.Cells;
            TF001 = cell[1].Text.Trim();
            TF002 = cell[2].Text.Trim();
            TF003 = cell[5].Text.Trim();
            TF004 = cell[4].Text.Trim();
            MOCCHECKSCOMMENTS = GRIDVIEWTextBox1.Text.ToString();
            MOCCHECKS = GRIDVIEWDropDownList1.SelectedValue.ToString();


            if (!string.IsNullOrEmpty(MOCCHECKSCOMMENTS))
            {
                ADDTBCOPTFCHECKMOC(TF001, TF002, TF003, TF004, null, MOCCHECKS, MOCCHECKSCOMMENTS);
                //MsgBox(TD001 + TD002 + TD003 + " " + MOCCHECKSCOMMENTS+" "+MOCCHECKS, this.Page, this);
            }

           
        }

        foreach (GridViewRow row in this.Grid2.Rows)
        {
            ((TextBox)row.FindControl("GRIDVIEWTextBox1")).Text = "";
        }

    }

    public void ADDPUR()
    {
        string TF001 = null;
        string TF002 = null;
        string TF003 = null;
        string TF004 = null;
        string PURCHECKSCOMMENTS = null;
        string PURCHECKS = null;

        foreach (GridViewRow gvr in this.Grid3.Rows)
        {
            var GRIDVIEW2TextBox1 = (TextBox)gvr.FindControl("GRIDVIEW2TextBox1");
            var GRIDVIEW2DropDownList1 = (DropDownList)gvr.FindControl("GRIDVIEW2DropDownList1");

            TableCellCollection cell = gvr.Cells;
            TF001 = cell[1].Text.Trim();
            TF002 = cell[2].Text.Trim();
            TF003 = cell[5].Text.Trim();
            TF004 = cell[4].Text.Trim();
            PURCHECKSCOMMENTS = GRIDVIEW2TextBox1.Text.ToString();
            PURCHECKS = GRIDVIEW2DropDownList1.SelectedValue.ToString();


            if (!string.IsNullOrEmpty(PURCHECKSCOMMENTS))
            {
                ADDTBCOPTDCHECKPUR(TF001, TF002, TF003, TF004, null, PURCHECKS, PURCHECKSCOMMENTS);
                //MsgBox(TD001 + TD002 + TD003 + " " + MOCCHECKSCOMMENTS+" "+MOCCHECKS, this.Page, this);
            }


        }

        foreach (GridViewRow row in this.Grid2.Rows)
        {
            ((TextBox)row.FindControl("GRIDVIEWTextBox1")).Text = "";
        }

    }

    public void ADDSALES()
    {
        string TF001 = null;
        string TF002 = null;
        string TF003 = null;
        string TF004 = null;
        string SALESCHECKSCOMMENTS = null;
      

        foreach (GridViewRow gvr in this.Grid4.Rows)
        {
            var GRIDVIEW4TextBox1 = (TextBox)gvr.FindControl("GRIDVIEW4TextBox1");
          

            TableCellCollection cell = gvr.Cells;
            TF001 = cell[1].Text.Trim();
            TF002 = cell[2].Text.Trim();
            TF003 = cell[5].Text.Trim();
            TF004 = cell[4].Text.Trim();
            SALESCHECKSCOMMENTS = GRIDVIEW4TextBox1.Text.ToString();
           


            if (!string.IsNullOrEmpty(SALESCHECKSCOMMENTS))
            {
                ADDTBCOPTDCHECKSALES(TF001, TF002, TF003, TF004, null, SALESCHECKSCOMMENTS);
                //MsgBox(TD001 + TD002 + TD003 + " " + MOCCHECKSCOMMENTS+" "+MOCCHECKS, this.Page, this);
            }


        }

        foreach (GridViewRow row in this.Grid2.Rows)
        {
            ((TextBox)row.FindControl("GRIDVIEWTextBox1")).Text = "";
        }

    }


    public string SEARCHROLES(string ACCOUNT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            SELECT  
                            [ID]
                            ,[ROLES]
                            ,[MV001]
                            ,[MV002]
                            FROM [TKBUSINESS].[dbo].[TBCOPTDCHECKROLES]
                            WHERE MV001 LIKE '{0}%'

                              ", ACCOUNT);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["ROLES"].ToString().Trim();
        }
        else
        {
            return "NOROLES";
        }

    }

    public void ADDTBCOPTFCHECKMOC(string TF001,
                                string TF002,
                                string TF003,
                                string TF004,
                                string MOCCHECKDATES,
                                string MOCCHECKS,
                                string MOCCHECKSCOMMENTS
                               )
    {
        MOCCHECKDATES= DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKBUSINESS].[dbo].[TBCOPTFCHECK]
                        (
                        [TF001]
                        ,[TF002]
                        ,[TF003]
                        ,[TF004]
                        ,[TF005]
                        ,[TF006]
                        ,[TF007]
                        ,[TF009]
                        ,[TF010]
                        ,[TF013]
                        ,[TF014]
                        ,[TF015]
                        ,[TF018]
                        ,[TF032]
                        ,[TF045]
                        ,[TF104]
                        ,[TE006]
                        ,[TE050]
                        ,[MOCCHECKDATES]
                        ,[MOCCHECKS]
                        ,[MOCCHECKSCOMMENTS]
                        ,[PURCHECKDATES]
                        ,[PURCHECKS]
                        ,[PURCHECKSCOMMENTS]
                        ,[SALESCHECKDATES]
                        ,[SALESCHECKSCOMMENTS]
                        )

                        SELECT 
                        [TF001]
                        ,[TF002]
                        ,[TF003]
                        ,[TF004]
                        ,[TF005]
                        ,[TF006]
                        ,[TF007]
                        ,[TF009]
                        ,[TF010]
                        ,[TF013]
                        ,[TF014]
                        ,[TF015]
                        ,[TF018]
                        ,[TF032]
                        ,[TF045]
                        ,[TF104]
                        ,[TE006]
                        ,[TE050]
                        ,@MOCCHECKDATES
                        ,@MOCCHECKS
                        ,@MOCCHECKSCOMMENTS
                        ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                        ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                        ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                        ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                        ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'

                        FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                        LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                        LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                        WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                        AND LTRIM(RTRIM(TF001))=@TF001
                        AND LTRIM(RTRIM(TF002))=@TF002
                        AND LTRIM(RTRIM(TF003))=@TF003
                        AND LTRIM(RTRIM(TF004))=@TF004
                   
                            ";


        m_db.AddParameter("@TF001", TF001);
        m_db.AddParameter("@TF002", TF002);
        m_db.AddParameter("@TF003", TF003);
        m_db.AddParameter("@TF004", TF004);
        m_db.AddParameter("@MOCCHECKDATES", MOCCHECKDATES);
        m_db.AddParameter("@MOCCHECKS", MOCCHECKS);
        m_db.AddParameter("@MOCCHECKSCOMMENTS", MOCCHECKSCOMMENTS);
      


        m_db.ExecuteNonQuery(cmdTxt);

    }

    public void ADDTBCOPTDCHECKPUR(string TF001,
                               string TF002,
                               string TF003,
                               string TF004,
                               string PURCHECKDATES,
                               string PURCHECKS,
                               string PURCHECKSCOMMENTS
                              )
    {
        PURCHECKDATES = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKBUSINESS].[dbo].[TBCOPTFCHECK]
                        (
                        [TF001]
                        ,[TF002]
                        ,[TF003]
                        ,[TF004]
                        ,[TF005]
                        ,[TF006]
                        ,[TF007]
                        ,[TF009]
                        ,[TF010]
                        ,[TF013]
                        ,[TF014]
                        ,[TF015]
                        ,[TF018]
                        ,[TF032]
                        ,[TF045]
                        ,[TF104]
                        ,[TE006]
                        ,[TE050]
                        ,[MOCCHECKDATES]
                        ,[MOCCHECKS]
                        ,[MOCCHECKSCOMMENTS]
                        ,[PURCHECKDATES]
                        ,[PURCHECKS]
                        ,[PURCHECKSCOMMENTS]
                        ,[SALESCHECKDATES]
                        ,[SALESCHECKSCOMMENTS]
                        )

                        SELECT 
                        [TF001]
                        ,[TF002]
                        ,[TF003]
                        ,[TF004]
                        ,[TF005]
                        ,[TF006]
                        ,[TF007]
                        ,[TF009]
                        ,[TF010]
                        ,[TF013]
                        ,[TF014]
                        ,[TF015]
                        ,[TF018]
                        ,[TF032]
                        ,[TF045]
                        ,[TF104]
                        ,[TE006]
                        ,[TE050]
                        ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES' 
                        ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                        ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                        ,@PURCHECKDATES
                        ,@PURCHECKS
                        ,@PURCHECKSCOMMENTS
                        ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                        ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'

                        FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                        LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                        LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                        WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                        AND LTRIM(RTRIM(TF001))=@TF001
                        AND LTRIM(RTRIM(TF002))=@TF002
                        AND LTRIM(RTRIM(TF003))=@TF003
                        AND LTRIM(RTRIM(TF004))=@TF004
                   
                            ";


        m_db.AddParameter("@TF001", TF001);
        m_db.AddParameter("@TF002", TF002);
        m_db.AddParameter("@TF003", TF003);
        m_db.AddParameter("@TF004", TF004);
        m_db.AddParameter("@PURCHECKDATES", PURCHECKDATES);
        m_db.AddParameter("@PURCHECKS", PURCHECKS);
        m_db.AddParameter("@PURCHECKSCOMMENTS", PURCHECKSCOMMENTS);



        m_db.ExecuteNonQuery(cmdTxt);

    }

    public void ADDTBCOPTDCHECKSALES(string TF001,
                               string TF002,
                               string TF003,
                               string TF004,
                               string SALESCHECKDATES,
                               string SALESCHECKSCOMMENTS
                              
                              )
    {
        SALESCHECKDATES = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKBUSINESS].[dbo].[TBCOPTFCHECK]
                        (
                        [TF001]
                        ,[TF002]
                        ,[TF003]
                        ,[TF004]
                        ,[TF005]
                        ,[TF006]
                        ,[TF007]
                        ,[TF009]
                        ,[TF010]
                        ,[TF013]
                        ,[TF014]
                        ,[TF015]
                        ,[TF018]
                        ,[TF032]
                        ,[TF045]
                        ,[TF104]
                        ,[TE006]
                        ,[TE050]
                        ,[MOCCHECKDATES]
                        ,[MOCCHECKS]
                        ,[MOCCHECKSCOMMENTS]
                        ,[PURCHECKDATES]
                        ,[PURCHECKS]
                        ,[PURCHECKSCOMMENTS]
                        ,[SALESCHECKDATES]
                        ,[SALESCHECKSCOMMENTS]
                        )

                        SELECT 
                        [TF001]
                        ,[TF002]
                        ,[TF003]
                        ,[TF004]
                        ,[TF005]
                        ,[TF006]
                        ,[TF007]
                        ,[TF009]
                        ,[TF010]
                        ,[TF013]
                        ,[TF014]
                        ,[TF015]
                        ,[TF018]
                        ,[TF032]
                        ,[TF045]
                        ,[TF104]
                        ,[TE006]
                        ,[TE050]
                        ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES' 
                        ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                        ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                        ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                        ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                        ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                        ,@SALESCHECKDATES
                        ,@SALESCHECKSCOMMENTS

                        FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                        LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                        LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                        WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                        AND LTRIM(RTRIM(TF001))=@TF001
                        AND LTRIM(RTRIM(TF002))=@TF002
                        AND LTRIM(RTRIM(TF003))=@TF003
                        AND LTRIM(RTRIM(TF004))=@TF004
                   
                            ";


        m_db.AddParameter("@TF001", TF001);
        m_db.AddParameter("@TF002", TF002);
        m_db.AddParameter("@TF003", TF003);
        m_db.AddParameter("@TF004", TF004);
        m_db.AddParameter("@SALESCHECKDATES", SALESCHECKDATES);
        m_db.AddParameter("@SALESCHECKSCOMMENTS", SALESCHECKSCOMMENTS);



        m_db.ExecuteNonQuery(cmdTxt);

    }
    public DataTable FINDCOPTCTC004(string TC001TC002)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            SELECT TOP 1 TC004
                            ,SUM(TC029+TC030) AS COPTCMONEYS 
                            FROM [TK].dbo.COPTC 
                            WHERE TC001+TC002='{0}' 
                            GROUP BY TC004

                              ", TC001TC002);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return null;
        }
    }

    public decimal FINDCREDITS(string MA001)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                           
                            SELECT MA001,ISNULL(SUM(NOT_TC_SUMFINAL),0) AS 'TOTALSUM'
                            FROM 
                            (
                            SELECT MA001,'未兌現應收票據' AS KIND
                            ,ISNULL(SUM(ROUND(ISNULL(TC003,0)*ISNULL(TC027,0),0)),0) AS NOT_TC_SUM  
                            ,ISNULL(SUM(ROUND(ISNULL(TC003,0)*ISNULL(TC027,0),0)),0)*MA091 AS NOT_TC_SUMFINAL  
                            FROM [TK].dbo.NOTTC AS NOTTC 
                            INNER JOIN [TK].dbo.COPMA AS COPMA ON TC013=MA001  
                            WHERE TC012 IN ('1','2','3','5','9','A')  
                            AND TC003 IS NOT NULL AND TC027 IS NOT NULL  
                            AND (MA065='{0}' OR MA001='{0}') 
                            GROUP BY MA001,MA091

                            UNION ALL
                            SELECT  MA001,'應收帳款'
                            ,ISNULL(SUM((TA041+TA042-TA058)*ISNULL(MQ010,0)),0)  AS ACR_TA_SUM  
                            ,ISNULL(SUM((TA041+TA042-TA058)*ISNULL(MQ010,0)),0)*MA092  AS ACR_TA_SUMFINAL  
                            FROM [TK].dbo.ACRTA AS ACRTA  
                            INNER JOIN [TK].dbo.COPMA AS COPMA ON TA004=MA001  
                            LEFT JOIN [TK].dbo.CMSMQ AS CMSMQ ON MQ001=TA001  
                            WHERE TA027='N' AND TA025='Y'  
                            AND (MA065='{0}' OR MA001='{0}') 
                            GROUP BY MA001,MA092

                            UNION ALL
                            SELECT  MA001,'未結帳銷貨'
                            ,ISNULL(SUM(ISNULL(TH037,0)+ISNULL(TH038,0)),0) AS TH013A 
                            ,ISNULL(SUM(ISNULL(TH037,0)+ISNULL(TH038,0)),0)*MA093 AS TH013AFINAL   
                            FROM [TK].dbo.COPTG AS COPTG 
                            INNER JOIN [TK].dbo.COPTH AS COPTH ON TG001=TH001 AND TG002=TH002  
                            INNER JOIN [TK].dbo.COPMA AS COPMA ON TG004=MA001  
                            WHERE TG023='Y' AND TH026='N' 
                            AND (MA065='{0}' OR MA001='{0}') 
                            AND TG034<>'Y'
                            GROUP BY MA001,MA093

                            UNION ALL
                            SELECT MA001,'未結帳銷退1'
                            ,ISNULL(SUM(ISNULL(TJ033,0)+ISNULL(TJ034,0)),0) AS TJ012A 
                            ,ISNULL(SUM(ISNULL(TJ033,0)+ISNULL(TJ034,0)),0)*MA093*-1 AS TJ012AFINAL   
                            FROM [TK].dbo.COPTI AS COPTI 
                            INNER JOIN [TK].dbo.COPTJ AS COPTJ ON TI001=TJ001 AND TI002=TJ002  
                            INNER JOIN [TK].dbo.COPTG AS COPTG ON TG001=TJ015 AND TG002=TJ016  
                            INNER JOIN [TK].dbo.COPMA AS COPMA ON MA001=TI004  
                            WHERE TI019='Y' AND TJ024='N' 
                            AND (MA065='{0}' OR MA001='{0}') 
                            AND TG034<>'Y'
                            GROUP BY MA001,MA093 

                            UNION ALL  
                            SELECT MA001,'未結帳銷退2'
                            ,ISNULL(SUM(ISNULL(TJ033,0)+ISNULL(TJ034,0)),0) AS TJ012A 
                            ,ISNULL(SUM(ISNULL(TJ033,0)+ISNULL(TJ034,0)),0)*MA093*-1 AS TJ012AFINAL    
                            FROM [TK].dbo.COPTI AS COPTI 
                            INNER JOIN [TK].dbo.COPTJ AS COPTJ ON TI001=TJ001 AND TI002=TJ002  
                            INNER JOIN [TK].dbo.COPMA AS COPMA ON MA001=TI004  
                            WHERE TI019='Y' AND TJ024='N' 
                            AND (MA065='{0}' OR MA001='{0}') 
                            AND (TJ015+TJ016='')  
                            GROUP BY MA001,MA093

                            UNION ALL
                            SELECT MA001,'未出貨訂單'
                            ,ISNULL(SUM(ROUND(ISNULL(TD011,0)*ISNULL(TC009,0)*(ISNULL(TD008,0)-ISNULL(TD009,0))*ISNULL(TD026,0),0)),0) AS TD011A  
                            ,ISNULL(SUM(ROUND(ISNULL(TD011,0)*ISNULL(TC009,0)*(ISNULL(TD008,0)-ISNULL(TD009,0))*ISNULL(TD026,0),0)),0)*MA094 AS TD011AFINAL   
                            FROM [TK].dbo.COPTC AS COPTC 
                            INNER JOIN [TK].dbo.COPTD AS COPTD ON TC001=TD001 AND TC002=TD002  
                            INNER JOIN [TK].dbo.COPMA AS COPMA ON TC004=MA001  
                            WHERE TC027='Y' AND TD016='N' 
                            AND (MA065='{0}' OR MA001='{0}') 
                            GROUP BY MA001,MA094

                            UNION ALL
                            SELECT MA001,'未歸還暫出'
                            ,ISNULL(SUM(ISNULL(TG012,0)*ISNULL(TF012,0)*(ISNULL(TG009,0)-ISNULL(TG021,0)-ISNULL(TG020,0))),0) AS TG012A  
                            ,ISNULL(SUM(ISNULL(TG012,0)*ISNULL(TF012,0)*(ISNULL(TG009,0)-ISNULL(TG021,0)-ISNULL(TG020,0))),0)*MA132 AS TG012AFINAL  
                            FROM [TK].dbo.INVTF INVTF 
                            INNER JOIN [TK].dbo.INVTG INVTG ON TF001=TG001 AND TG002=TF002  
                            INNER JOIN [TK].dbo.COPMA AS COPMA ON TF005=MA001  
                            INNER JOIN [TK].dbo.CMSMQ CMSMQ ON MQ001=TG001  
                            WHERE TF020='Y' AND TF004='1' AND TG024='N' 
                            AND (MA065='{0}' OR MA001='{0}') 
                            AND MQ003='13'  and (TG014='' AND TG015='' AND TG016='')
                            GROUP BY MA001,MA132
                            ) AS TEMP
                            GROUP BY MA001

                              ", MA001);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return Convert.ToDecimal(dt.Rows[0]["TOTALSUM"].ToString());
        }
        else
        {
            return 0;
        }
    }

    public decimal FINDCOPMATOTALLIMITS(string MA001)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            SELECT 
                            MA033 信用額度
                            ,MA034 可超出率
                            ,MA091 未兌現應收票據比率
                            ,MA092 應收帳款比率
                            ,MA093 未結帳銷貨金額比率
                            ,MA094 未出貨訂單金額比率
                            ,MA132 未歸還暫出金額比率
                            ,MA033*(1+MA034) AS 'TOTALLIMITS'
                            FROM [TK].dbo.COPMA
                            WHERE MA001='{0}'
                           

                              ", MA001);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return Convert.ToDecimal(dt.Rows[0]["TOTALLIMITS"].ToString());
        }
        else
        {
            return 0;
        }
    }

    public DataTable FINDCOPTE(string TE001002003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            SELECT TE007
                            ,ISNULL((SELECT SUM(TF014) FROM [TK].dbo.COPTF WHERE TF001=TE001 AND TF002=TE002 AND TF003=TE003),0) AS COPTEMONEYS
                            ,ISNULL((SELECT SUM(TC029+TC030) FROM [TK].dbo.COPTC WHERE TC001=TE001 AND TC002=TE002),0) AS COPTCMONEYS
                            FROM [TK].dbo.COPTE
                            WHERE TE001+TE002+TE003='{0}'
                           

                              ", TE001002003);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return null;
        }
    }

    private void BindGrid7(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

       
        //核單
        if (!string.IsNullOrEmpty(DropDownList10.Text))
        {
            if (DropDownList10.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TE029='N' ");
            }
            else if (DropDownList10.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TE029='Y' ");
            }
        }

        //年度
        if (!string.IsNullOrEmpty(TextBox26.Text))
        {
            QUERYS.AppendFormat(@" AND TE002 LIKE '{0}%'", TextBox26.Text.Trim());

        }

        //訂單單號
        if (!string.IsNullOrEmpty(TextBox22.Text))
        {
            QUERYS.AppendFormat(@" AND TE002 LIKE '{0}%'", TextBox22.Text.Trim());

        }

        //客戶名稱
        if (!string.IsNullOrEmpty(TextBox23.Text))
        {
            QUERYS.AppendFormat(@" AND TC053 LIKE '%{0}%'", TextBox23.Text.Trim());

        }


        cmdTxt.AppendFormat(@" 
                                SELECT 
                                LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
                                ,LTRIM(RTRIM(TE001))+LTRIM(RTRIM(TE002))+LTRIM(RTRIM(TE003)) AS 'TF123'
                                ,*
                                ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND  TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
                                ,COPTF.UDF01 AS 'COPTFUDF01'

                                FROM [TK].dbo.COPTE
                                LEFT JOIN [TK].dbo.COPTF ON TE001=TF001 AND TE002=TF002 AND TE003=TF003 
                                LEFT JOIN [TK].dbo.COPTC ON TC001=TE001 AND TC002=TE002
                                LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                                WHERE 1=1
                                AND ISNULL(TF004,'')=''
                                {0}
                               
                                

                                ", QUERYS.ToString());




        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



        Grid7.DataSource = dt;
        Grid7.DataBind();
    }

    protected void grid_PageIndexChanging7(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid7_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ///Get the button that raised the event
            Button btn = (Button)e.Row.FindControl("GRIDVIEW7Button1");
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue = btn.CommandArgument;
            DataRowView row = (DataRowView)e.Row.DataItem;
            Button lbtnName = (Button)e.Row.FindControl("GRIDVIEW7Button1");
            ExpandoObject param = new { ID = Cellvalue }.ToExpando(); 


        }







    }

    protected void Grid7_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "GRIDVIEW7Button1")
        {
            //檢查是否已送單，簽核中
            DataTable DT = CHECK_TB_WKF_TASK(e.CommandArgument.ToString());

            if (DT != null && DT.Rows.Count >= 1)
            {
                MsgBox(e.CommandArgument.ToString() + " 重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
            }
            else
            {
                CHECKTBCOPTFCHECK7(e.CommandArgument.ToString());

                //用訂單變更找出客代TC004
                DataTable DTCOPTE = FINDCOPTE(e.CommandArgument.ToString());
                string TC004 = DTCOPTE.Rows[0]["TE007"].ToString();
                //訂單金額
                decimal COPTCMONEYS = Convert.ToDecimal(DTCOPTE.Rows[0]["COPTCMONEYS"].ToString());
                int INTCOPTCMONEYS = Convert.ToInt32(COPTCMONEYS);
                //訂單變更金額
                decimal COPTEMONEYS = Convert.ToDecimal(DTCOPTE.Rows[0]["COPTEMONEYS"].ToString());
                int INTCOPTEMONEYS = Convert.ToInt32(COPTEMONEYS);
                //用客代找出信用額度設定上限
                decimal TOTALLIMITS = FINDCOPMATOTALLIMITS(TC004);
                int INTTOTALLIMITS = Convert.ToInt32(TOTALLIMITS);
                //用客代找出目前已用的信用額度
                decimal TOTALCREDITS = FINDCREDITS(TC004);
                int INTTOTALCREDITS = Convert.ToInt32(TOTALCREDITS);

                if (INTTOTALLIMITS < (INTCOPTEMONEYS + (-1 * INTTOTALCREDITS) + INTCOPTCMONEYS))
                {
                    MsgBox(e.CommandArgument.ToString() + " 訂單變更金額=" + INTCOPTEMONEYS.ToString("0,0") + " \r\n原訂單金額=" + INTCOPTCMONEYS.ToString("0,0") + " \r\n客代:" + TC004 + " \r\n目前設定的信用額度=" + INTTOTALLIMITS.ToString("0,0") + " \r\n已花費的信用額度=" + INTTOTALCREDITS.ToString("0,0"), this.Page, this);
                }
            }

           

            //MsgBox(e.CommandArgument.ToString(), this.Page, this);           
        }

    }


    public void OnBeforeExport7(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

    }

    public DataTable CHECK_TB_WKF_TASK(string TC001TC002)
    {
        string TC001 = TC001TC002.Substring(0, 4);
        string TC002 = TC001TC002.Substring(4, 11);

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                           SELECT CURRENT_DOC
                            ,CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE001""]/@fieldValue)[1]', 'nvarchar(max)') AS TE001Value
                            , CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE002""]/@fieldValue)[1]', 'nvarchar(max)') AS TE002Value
                            , TASK_STATUS
                            FROM[UOF].[dbo].TB_WKF_TASK
                            WHERE  1 = 1
                            AND TASK_STATUS = '1'
                            AND CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE001""]/@fieldValue)[1]', 'nvarchar(max)') LIKE '%{0}%'
                            AND CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE002""]/@fieldValue)[1]', 'nvarchar(max)') LIKE '%{1}%'



                              ", TC001, TC002);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return null;
        }
    }
    public DataTable CHECK_COPTF_ZINVMBBAKING(string TE001, string TE002, string TE003)
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();


        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();



            cmdTxt.AppendFormat(@" 
                                SELECT *
                                FROM [TK].dbo.COPTF
                                WHERE TF001='{0}' AND TF002='{1}' AND TF003='{2}'
                                AND TF005 IN (
                                SELECT 
                                [MB001]
                                FROM [TK].[dbo].[ZINVMBBAKING]
                                )
                              
                                ", TE001, TE002, TE003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));



            if (dt.Rows.Count >= 1)
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
        {

        }
    }

    public DataTable SEARCH_MOCMANULINEMB001LIKES()
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();


        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();



            cmdTxt.AppendFormat(@" 
                                SELECT [MB001]
                                 FROM [TKMOC].[dbo].[MOCMANULINEMB001LIKES]                              
                                ");




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt.Rows.Count >= 1)
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
        {

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
        //this.Session["SDATE"] = txtDate1.Text.Trim();
        //this.Session["EDATE"] = txtDate2.Text.Trim();
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        SETEXCEL();
    }
    protected void btn3_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=test.xls");
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
        HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=big5>");
        HttpContext.Current.Response.Write("<head><meta http-equiv=Content-Type content=text/html;charset=big5></head>");
        Response.Charset = "big5";
        Response.ContentType = "application/excel";


        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        Grid1.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
    protected void MyButtonClick(object sender, System.EventArgs e)
    {


    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid("");
        BindGrid2("");
        BindGrid3("");
        BindGrid4("");
        BindGrid5("");

    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        BindGrid("");
        BindGrid2("");
        BindGrid3("");
        BindGrid4("");
        BindGrid5("");

    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        BindGrid("");
        BindGrid2("");
        BindGrid3("");
        BindGrid4("");
        BindGrid5("");

    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        ADDMOC();

        BindGrid("");
        BindGrid2("");
        BindGrid3("");
        BindGrid4("");
        BindGrid5("");

    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        ADDPUR();

        BindGrid("");
        BindGrid2("");
        BindGrid3("");
        BindGrid4("");
        BindGrid5("");

    }

    protected void Button7_Click(object sender, EventArgs e)
    {
       
        BindGrid("");
        BindGrid2("");
        BindGrid3("");
        BindGrid4("");
        BindGrid5("");

    }

    protected void Button8_Click(object sender, EventArgs e)
    {
        ADDSALES();

        BindGrid("");
        BindGrid2("");
        BindGrid3("");
        BindGrid4("");
        BindGrid5("");

    }

    protected void Button10_Click(object sender, EventArgs e)
    {

        BindGrid6("");      

    }
    protected void Button11_Click(object sender, EventArgs e)
    {

        BindGrid7("");

    }

    #endregion
}