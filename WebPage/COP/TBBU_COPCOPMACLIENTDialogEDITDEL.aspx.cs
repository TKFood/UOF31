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


public partial class CDS_WebPage_TBBU_COPCOPMACLIENTDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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


            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHTCOPCOPMACLIENT(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE();

        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE();

        SEARCHTCOPCOPMACLIENT(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELCOPCOPMACLIENT(lblParam.Text);
    }


    #endregion

    #region FUNCTION


    public void SEARCHTCOPCOPMACLIENT(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                       SELECT [ID]
                        ,[MA001]
                        ,[MA002]
                        ,[CLIENTS]
                        ,[OPERATIONS]
                        ,[COMMENTS]
                        FROM [TKBUSINESS].[dbo].[COPCOPMACLIENT]
                        WHERE [ID]=@ID
                        ORDER BY MA001
                        ";
        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["MA001"].ToString();
            TextBox2.Text = dt.Rows[0]["MA002"].ToString();
            TextBox3.Text = dt.Rows[0]["CLIENTS"].ToString();
            TextBox4.Text = dt.Rows[0]["OPERATIONS"].ToString();
            TextBox5.Text = dt.Rows[0]["COMMENTS"].ToString();


        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string MA001 = TextBox1.Text;
        string MA002 = TextBox2.Text;
        string CLIENTS = TextBox3.Text;
        string OPERATIONS = TextBox4.Text;
        string COMMENTS = TextBox5.Text;

        if (!string.IsNullOrEmpty(ID) )
        {
          UPDATECOPCOPMACLIENT(ID, MA001, MA002, CLIENTS, OPERATIONS, COMMENTS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATECOPCOPMACLIENT(string ID, string MA001, string MA002, string CLIENTS, string OPERATIONS, string COMMENTS)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKBUSINESS].[dbo].[COPCOPMACLIENT]
                        SET 
                        [MA001]=@MA001
                        ,[MA002]=@MA002
                        ,[CLIENTS]=@CLIENTS
                        ,[OPERATIONS]=@OPERATIONS
                        ,[COMMENTS]=@COMMENTS
                        WHERE [ID]=@ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@MA001", MA001);
        m_db.AddParameter("@MA002", MA002);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@OPERATIONS", OPERATIONS);
        m_db.AddParameter("@COMMENTS", COMMENTS);

        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DELCOPCOPMACLIENT(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE [TKBUSINESS].[dbo].[COPCOPMACLIENT] WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

    
    #endregion


}
