using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CDS_WebPage_TBBU_TBPURMOQDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
                SEARCHTBPURMOQ(lblParam.Text);
            }

        }

    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE(lblParam.Text);

        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE(lblParam.Text);

        SEARCHTBPURMOQ(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBPURMOQ(lblParam.Text);
    }


    #endregion

    #region FUNCTION
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[KINDS] FROM [TKBUSINESS].[dbo].[TBPURMOQKINDS] ORDER BY [ID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "KINDS";
            DropDownList1.DataValueField = "KINDS";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }

    public void SEARCHTBPURMOQ(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                        SELECT 
                        [ID],[KINDS],[NAMES],[MOQS],[INDAYS],[COMMENTS]
                        FROM [TKBUSINESS].[dbo].[TBPURMOQ]
                        WHERE [ID]=@ID
                        ";
        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.SelectedValue= dt.Rows[0]["KINDS"].ToString();
            TextBox1.Text = dt.Rows[0]["NAMES"].ToString();
            TextBox2.Text = dt.Rows[0]["MOQS"].ToString();
            TextBox3.Text = dt.Rows[0]["INDAYS"].ToString();
            TextBox4.Text = dt.Rows[0]["COMMENTS"].ToString();


        }




    }

    public void UPDATE(string MID)
    {
        string ID = MID;
        string KINDS = DropDownList1.SelectedValue.ToString();
        string NAMES = TextBox1.Text.Trim();
        string MOQS = TextBox2.Text.Trim();
        string INDAYS = TextBox3.Text.Trim();
        string COMMENTS = TextBox4.Text.Trim();


        if (!string.IsNullOrEmpty(ID) )
        {
            UPDATETBPURMOQ(ID, KINDS, NAMES, MOQS, INDAYS, COMMENTS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBPURMOQ(string ID, string KINDS, string NAMES, string MOQS, string INDAYS,string COMMENTS)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKBUSINESS].[dbo].[TBPURMOQ]
                        SET
                        [KINDS]=@KINDS
                        ,[NAMES]=@NAMES
                        ,[MOQS]=@MOQS
                        ,[INDAYS]=@INDAYS
                        ,[COMMENTS]=@COMMENTS
                        WHERE [ID]=@ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@KINDS", KINDS);
        m_db.AddParameter("@NAMES", NAMES);
        m_db.AddParameter("@MOQS", MOQS);
        m_db.AddParameter("@INDAYS", INDAYS);
        m_db.AddParameter("@COMMENTS", COMMENTS);



        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DELTBPURMOQ(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE [TKBUSINESS].[dbo].[TBPURMOQ] WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

    
    #endregion


}
