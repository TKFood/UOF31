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


public partial class CDS_WebPage_TBBU_TBPURMOQDialogADD : Ede.Uof.Utility.Page.BasePage
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
            BindDropDownList();

            ////接收主頁面傳遞之參數
            //lblParam.Text = Request["ID"];


            //if (!string.IsNullOrEmpty(lblParam.Text))
            //{
            //   // SEARCHTCOPCONDTIONS(lblParam.Text);
            //}

        }
        
    }

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


    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        Dialog.SetReturnValue2("ADD");

        ADD();

        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        Dialog.SetReturnValue2("ADD");

        ADD();

       
    }

    #endregion

    #region FUNCTION


    public void ADD()
    {
        string KINDS = DropDownList1.SelectedValue.ToString();
        string NAMES = TextBox1.Text.Trim();
        string MOQS = TextBox2.Text.Trim();
        string INDAYS = TextBox3.Text.Trim();
        string COMMENTS=TextBox4.Text.Trim();

        if ( !string.IsNullOrEmpty(KINDS))
        {

            ADDTBPURMOQ(KINDS, NAMES, MOQS, INDAYS, COMMENTS);
        }

        //Dialog.SetReturnValue2("ADD");
        //Dialog.Close(this);
    }
    public void ADDTBPURMOQ(string KINDS, string NAMES, string MOQS, string INDAYS,string COMMENTS)
    {
      

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        try
        {
            string cmdTxt = @"  
                            INSERT INTO [TKBUSINESS].[dbo].[TBPURMOQ]
                            ([KINDS],[NAMES],[MOQS],[INDAYS],[COMMENTS])
                            VALUES
                            (@KINDS,@NAMES,@MOQS,@INDAYS,@COMMENTS)

                            ";



            m_db.AddParameter("@KINDS", KINDS);
            m_db.AddParameter("@NAMES", NAMES);
            m_db.AddParameter("@MOQS", MOQS);
            m_db.AddParameter("@INDAYS", INDAYS);
            m_db.AddParameter("@COMMENTS", COMMENTS);

            m_db.ExecuteNonQuery(cmdTxt);

            Label8.Text = "成功";
        }
        catch
        {
            Label8.Text = "新增失敗";
        }
        finally
        {

        }

        

    }

   

    #endregion


}
