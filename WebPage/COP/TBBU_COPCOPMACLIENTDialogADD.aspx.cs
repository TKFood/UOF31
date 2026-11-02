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


public partial class CDS_WebPage_TBBU_COPCOPMACLIENTDialogADD : Ede.Uof.Utility.Page.BasePage
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


    public void ADD()
    {
        //Guid ID = Guid.NewGuid();
        //string SERNO = "";
        string MA001 = TextBox1.Text;
        string MA002 = TextBox2.Text;
        string CLIENTS = TextBox3.Text;
        string OPERATIONS = TextBox4.Text;
        string COMMENTS = TextBox5.Text;

        int ID = FINDMAXSERNO();
      

        if ( !string.IsNullOrEmpty(MA001) && !string.IsNullOrEmpty(MA002))
        {

            ADDCOPCOPMACLIENT(ID.ToString(), MA001, MA002, CLIENTS, OPERATIONS, COMMENTS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDCOPCOPMACLIENT(string ID,string MA001, string MA002, string CLIENTS, string OPERATIONS, string COMMENTS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKBUSINESS].[dbo].[COPCOPMACLIENT]
                        ([ID],[MA001],[MA002],[CLIENTS],[OPERATIONS],[COMMENTS])
                        VALUES
                        (@ID,@MA001,@MA002,@CLIENTS,@OPERATIONS,@COMMENTS)
                       
                            ";



        m_db.AddParameter("@ID", ID);   
        m_db.AddParameter("@MA001", MA001);
        m_db.AddParameter("@MA002", MA002);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@OPERATIONS", OPERATIONS);
        m_db.AddParameter("@COMMENTS", COMMENTS);

        m_db.ExecuteNonQuery(cmdTxt);

    }

    public int FINDMAXSERNO()
    {
        DataTable dt = new DataTable();
     
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT MAX(CONVERT(INT,ID))+1 AS ID FROM [TKBUSINESS].[dbo].[COPCOPMACLIENT]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if(dt.Rows.Count>0)
        {
            return Convert.ToInt32(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    #endregion


}
