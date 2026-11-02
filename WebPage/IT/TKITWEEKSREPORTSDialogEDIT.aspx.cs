using Ede.Uof.EIP.SystemInfo;
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


public partial class CDS_WebPage_TKITWEEKSREPORTSDialogEDIT : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        ROLES = SEARCHROLES(ACCOUNT.Trim());



        //不顯示子視窗的按鈕
        //確定
        //((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        //確定後繼續
        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;
        //關閉
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


            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHITWEEKSREPORTS(lblParam.Text);
            }

        }

    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        UPDATEITWEEKSREPORTS(lblParam.Text.ToString(), TextBox1.Text.ToString(), TextBox2.Text.ToString(), TextBox3.Text.ToString());

        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        Dialog.SetReturnValue2("OK");
       
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        UPDATEITWEEKSREPORTS(lblParam.Text.ToString(), TextBox1.Text.ToString(), TextBox2.Text.ToString(), TextBox3.Text.ToString());
        //設定回傳值並關閉視窗
        Dialog.SetReturnValue2("OK");

        Dialog.Close(this);
        //SEARCHCOPTD(lblParam.Text);
    }


    #endregion

    #region FUNCTION


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
                            WHERE MV001 ='{0}'

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
    public void SEARCHITWEEKSREPORTS(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                        SELECT 
                        [ID]
                        ,[NAMES]
                        ,[WYEARS]
                        ,[WEEKS]
                        ,[SDATES]
                        ,[EDATES]
                        ,[COMMENTS]
                        ,[NOTFINISHEDS]
                        ,[PLANWORKS]
                        FROM [TKIT].[dbo].[ITWEEKSREPORTS]
                        WHERE [ID]=@ID
                        ";
        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["COMMENTS"].ToString();
            TextBox2.Text = dt.Rows[0]["NOTFINISHEDS"].ToString();
            TextBox3.Text = dt.Rows[0]["PLANWORKS"].ToString();

        }




    }

   



    public void UPDATEITWEEKSREPORTS(string ID, string COMMENTS,string NOTFINISHEDS, string PLANWORKS)
    {        

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                       
                        UPDATE [TKIT].[dbo].[ITWEEKSREPORTS]
                        SET [COMMENTS]=@COMMENTS,[NOTFINISHEDS]=@NOTFINISHEDS,[PLANWORKS]=@PLANWORKS
                        WHERE [ID]=@ID  
                        ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@NOTFINISHEDS", NOTFINISHEDS);
        m_db.AddParameter("@PLANWORKS", PLANWORKS);


        m_db.ExecuteNonQuery(cmdTxt);

    }


    #endregion




}
