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


public partial class CDS_WebPage_TBBU_TBPROJECTSSALESDialogADD : Ede.Uof.Utility.Page.BasePage
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




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();

        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();

       
    }

    #endregion

    #region FUNCTION
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SALESFOCUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[STORES] FROM [TKBUSINESS].[dbo].[STORESKIND] ORDER BY ID";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "STORES";
            DropDownList1.DataValueField = "STORES";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }

    public void ADD()
    { 
        string YEARS = TextBox1.Text;
        string WEEKS = TextBox2.Text;      
        string STORES = DropDownList1.SelectedValue.Trim();
        string NAMES = TextBox3.Text;
        string DAYS = TextBox5.Text;
        string TARGETS = TextBox4.Text;
        string FEES = TextBox7.Text;
        string ITEMS = TextBox6.Text;


        if ( !string.IsNullOrEmpty(YEARS)&& !string.IsNullOrEmpty(WEEKS))
        {

            ADDTBPROJECTSSALES(YEARS, WEEKS, STORES, NAMES, DAYS, TARGETS, FEES, ITEMS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDTBPROJECTSSALES(string YEARS, string WEEKS, string STORES, string NAMES, string DAYS, string TARGETS, string FEES, string ITEMS)
    {
        Label8.Text = "";

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        try
        {
            string cmdTxt = @"  
                            INSERT INTO [TKBUSINESS].[dbo].[TBPROJECTSSALES]
                            ([YEARS],[WEEKS],[DAYS],[STORES],[NAMES],[TARGETS],[FEES],[ITEMS])
                            VALUES
                            (@YEARS,@WEEKS,@DAYS,@STORES,@NAMES,@TARGETS,@FEES,@ITEMS)

                            ";



            m_db.AddParameter("@YEARS", YEARS);
            m_db.AddParameter("@WEEKS", WEEKS);
            m_db.AddParameter("@STORES", STORES);
            m_db.AddParameter("@NAMES", NAMES);
            m_db.AddParameter("@DAYS", DAYS);
            m_db.AddParameter("@TARGETS", TARGETS);
            m_db.AddParameter("@FEES", FEES);
            m_db.AddParameter("@ITEMS", ITEMS);

   


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
