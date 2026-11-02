using Ede.Uof.Utility.Component;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Globalization;


public partial class CDS_WebPage_TKRESEARCH_COSTDialogMATS : Ede.Uof.Utility.Page.BasePage
{
    

    protected void Page_Load(object sender, EventArgs e)
    {

        //不顯示子視窗的按鈕
        //((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;
        //((Master_DialogMasterPage)this.Master).Button3Text = string.Empty;


        //設定回傳值
        Dialog.SetReturnValue2("");

        //註冊Dialog的Button 狀態
        ((Master_DialogMasterPage)this.Master).Button1CausesValidation = false;
        ((Master_DialogMasterPage)this.Master).Button1AutoCloseWindow = false;
        ((Master_DialogMasterPage)this.Master).Button1OnClick += CDS_WebPage_Dialog_Button1OnClick;
        ((Master_DialogMasterPage)this.Master).Button2OnClick += Button2OnClick;

        if (!IsPostBack)
        {
            //接收主頁面傳遞之參數
            lblParam.Text = Request["ID"];
            BindDropDownList();

            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                BindGrid1(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);        
        
        Dialog.SetReturnValue2("REFRESH");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);      

        SEARCH_TBCOSTRECORDS(lblParam.Text);
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        string MMB001 = lblParam.Text;
        string MB002 = TextBox1.Text;
        string MB003 = TextBox2.Text;
        string COSTMAT = TextBox3.Text;

        ADD_TBCOSTRECORDSMAT(MMB001, MB002, MB003, COSTMAT);
        UPDATE_TBCOSTRECORDS_AFTER_ADDDEL();

        TextBox1.Text = null;
        TextBox2.Text = null;
        TextBox3.Text = null;

        BindGrid1(lblParam.Text);
    }
    #endregion

    #region FUNCTION
    private void BindDropDownList()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("PARAID", typeof(String));
        //dt.Columns.Add("PARANAME", typeof(String));

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='人工記錄成本' ORDER BY  [ID]  ";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    DropDownList1.DataSource = dt;
        //    DropDownList1.DataTextField = "PARANAME";
        //    DropDownList1.DataValueField = "PARANAME";
        //    DropDownList1.DataBind();

        //}
        //else
        //{

        //}
    }

    private void BindGrid1(string MMB001)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder SQUERY = new StringBuilder();



        if (!string.IsNullOrEmpty(MMB001))
        {
            cmdTxt.AppendFormat(@"
                              SELECT
                                [MMB001]
                                ,[MB001]
                                ,[MB002]
                                ,[MB003]
                                ,[COSTMAT]
                                FROM [TKRESEARCH].[dbo].[TBCOSTRECORDSMAT]
                                WHERE [MMB001]={0}
                                ORDER BY MB001
 

                                    ", MMB001);

            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            Grid1.DataSource = dt;
            Grid1.DataBind();

        }
        else
        {

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
            //Button1
            //Get the button that raised the event
            Button btn = (Button)e.Row.FindControl("GV1DELETE");
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue = btn.CommandArgument;
            DataRowView row = (DataRowView)e.Row.DataItem;
            Button lbtnName = (Button)e.Row.FindControl("GV1DELETE");
            ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            

           

        }



    }
    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "GV1DELETE")
        {           
            string MB002 =(e.CommandArgument).ToString();
            DELETE_TBCOSTRECORDSMAT(MB002);
            UPDATE_TBCOSTRECORDS_AFTER_ADDDEL();

            BindGrid1(lblParam.Text);
            //MsgBox("DELETE "+ MB002, this.Page, this);
        }


    }

    public void SEARCH_TBCOSTRECORDS(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                           
                      
                        ";


        m_db.AddParameter("@MB001", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));


    }

    public void UPDATE(string ID)
    {
        //string SERNO = lblParam.Text;
        //string MB001 = TextBox1.Text;
        //string MB002 = TextBox2.Text;
        //string MB003 = TextBox3.Text;
        //string COSTROW = TextBox4.Text;
        //string COSTHR = TextBox5.Text;
        //string COSTMANU = TextBox6.Text;
        //string COSTPRO = TextBox7.Text;
        //string COMMEMTS = TextBox9.Text;
        //string ISCLOSED = DropDownList1.Text;
        //string COSTMAT = TextBox10.Text;

        //if (!string.IsNullOrEmpty(MB001))
        //{
        //    UPDATE_TBCOSTRECORDS(
        //                MB001, MB002, MB003, COSTROW, COSTMAT,COSTHR, COSTMANU, COSTPRO, COMMEMTS, ISCLOSED
        //                );
        //}

        //Dialog.SetReturnValue2("REFRESH");
    }
   

 

    public void ADD_TBCOSTRECORDSMAT(string MMB001,  string MB002, string MB003, string COSTMAT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKRESEARCH].[dbo].[TBCOSTRECORDSMAT]
                        ([MMB001],[MB002],[MB003],[COSTMAT])
                        VALUES
                        (@MMB001,@MB002,@MB003,@COSTMAT)
                            ";

        m_db.AddParameter("@MMB001", MMB001);
        m_db.AddParameter("@MB002", MB002);
        m_db.AddParameter("@MB003", MB003);
        m_db.AddParameter("@COSTMAT", COSTMAT);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void UPDATE_TBCOSTRECORDS_AFTER_ADDDEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKRESEARCH].[dbo].[TBCOSTRECORDS]
                        SET [COSTMAT]=(SELECT SUM([COSTMAT]) FROM [TKRESEARCH].[dbo].[TBCOSTRECORDSMAT] WHERE [TBCOSTRECORDSMAT].MMB001=[TBCOSTRECORDS].MB001)
                        WHERE EXISTS (SELECT 1 FROM [TKRESEARCH].[dbo].[TBCOSTRECORDSMAT] WHERE [TBCOSTRECORDSMAT].MMB001=[TBCOSTRECORDS].MB001)

                            ";

    /*    m_db.AddParameter("@MMB001", MMB001)*/;
       

        m_db.ExecuteNonQuery(cmdTxt);
    }
    public void DELTBDEVNEW(string ID)
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"  DELETE [TKRESEARCH].[dbo].[TBDEVNEW]  WHERE [SERNO]=@ID
        //                    ";

        //m_db.AddParameter("@ID", ID);

        //m_db.ExecuteNonQuery(cmdTxt);


        //Dialog.SetReturnValue2("NeedPostBack");
        //Dialog.Close(this);
    }

    public void DELETE_TBCOSTRECORDSMAT(string MB002)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                         DELETE [TKRESEARCH].[dbo].[TBCOSTRECORDSMAT]
                         WHERE MB002=@MB002

                            ";

        m_db.AddParameter("@MB002", MB002);
      

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void MsgBox(String ex, Page pg, Object obj)
    {
        //MsgBox("Button1", this.Page, this);
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }

    #endregion


}
