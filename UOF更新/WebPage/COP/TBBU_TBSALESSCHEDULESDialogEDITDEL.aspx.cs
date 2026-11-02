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


public partial class CDS_WebPage_TBBU_TBSALESSCHEDULESDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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
                SEARCHTBSALESSCHEDULES(lblParam.Text);
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

        SEARCHTBSALESSCHEDULES(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBSALESSCHEDULES(lblParam.Text);
        SEARCHTBSALESSCHEDULES(lblParam.Text);
    }


    #endregion

    #region FUNCTION


    public void SEARCHTBSALESSCHEDULES(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                        SELECT 
                        [ID],[DEALER],[OWNERS],[EVENTS],[MONTHS1],[MONTHS2],[MONTHS3],[MONTHS4],[MONTHS5],[MONTHS6],[MONTHS7],[MONTHS8],[MONTHS9],[MONTHS10],[MONTHS11],[MONTHS12]
                        FROM [TKBUSINESS].[dbo].[TBSALESSCHEDULES]W
                        WHERE [ID]=@ID
                        ";
        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["DEALER"].ToString();
            TextBox2.Text = dt.Rows[0]["OWNERS"].ToString();
            TextBox3.Text = dt.Rows[0]["EVENTS"].ToString();
            TextBox4.Text = dt.Rows[0]["MONTHS1"].ToString();
            TextBox5.Text = dt.Rows[0]["MONTHS2"].ToString();
            TextBox6.Text = dt.Rows[0]["MONTHS3"].ToString();
            TextBox7.Text = dt.Rows[0]["MONTHS4"].ToString();
            TextBox8.Text = dt.Rows[0]["MONTHS5"].ToString();
            TextBox9.Text = dt.Rows[0]["MONTHS6"].ToString();
            TextBox10.Text = dt.Rows[0]["MONTHS7"].ToString();
            TextBox11.Text = dt.Rows[0]["MONTHS8"].ToString();
            TextBox12.Text = dt.Rows[0]["MONTHS9"].ToString();
            TextBox13.Text = dt.Rows[0]["MONTHS10"].ToString();
            TextBox14.Text = dt.Rows[0]["MONTHS11"].ToString();
            TextBox15.Text = dt.Rows[0]["MONTHS12"].ToString();



        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string DEALER = TextBox1.Text;
        string OWNERS = TextBox2.Text;
        string EVENTS = TextBox3.Text;
        string MONTHS1 = TextBox4.Text;
        string MONTHS2 = TextBox5.Text;
        string MONTHS3 = TextBox6.Text;
        string MONTHS4 = TextBox7.Text;
        string MONTHS5 = TextBox8.Text;
        string MONTHS6 = TextBox9.Text;
        string MONTHS7 = TextBox10.Text;
        string MONTHS8 = TextBox11.Text;
        string MONTHS9 = TextBox12.Text;
        string MONTHS10 = TextBox13.Text;
        string MONTHS11 = TextBox14.Text;
        string MONTHS12 = TextBox15.Text;


        if (!string.IsNullOrEmpty(ID) )
        {
            UPDATETBSALESSCHEDULES(ID, DEALER, OWNERS, EVENTS, MONTHS1, MONTHS2, MONTHS3, MONTHS4, MONTHS5, MONTHS6, MONTHS7, MONTHS8, MONTHS9, MONTHS10, MONTHS11, MONTHS12);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBSALESSCHEDULES(string ID, string DEALER, string OWNERS, string EVENTS, string MONTHS1, string MONTHS2, string MONTHS3, string MONTHS4, string MONTHS5, string MONTHS6, string MONTHS7, string MONTHS8, string MONTHS9, string MONTHS10, string MONTHS11, string MONTHS12)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKBUSINESS].[dbo].[TBSALESSCHEDULES]
                        SET [DEALER]=@DEALER,[OWNERS]=@OWNERS,[EVENTS]=@EVENTS,[MONTHS1]=@MONTHS1,[MONTHS2]=@MONTHS2,[MONTHS3]=@MONTHS3,[MONTHS4]=@MONTHS4,[MONTHS5]=@MONTHS5,[MONTHS6]=@MONTHS6,[MONTHS7]=@MONTHS7,[MONTHS8]=@MONTHS8,[MONTHS9]=@MONTHS9,[MONTHS10]=@MONTHS10,[MONTHS11]=@MONTHS11,[MONTHS12]=@MONTHS12
                        WHERE [ID]=@ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@DEALER", DEALER);
        m_db.AddParameter("@OWNERS", OWNERS);
        m_db.AddParameter("@EVENTS", EVENTS);
        m_db.AddParameter("@MONTHS1", MONTHS1);
        m_db.AddParameter("@MONTHS2", MONTHS2);
        m_db.AddParameter("@MONTHS3", MONTHS3);
        m_db.AddParameter("@MONTHS4", MONTHS4);
        m_db.AddParameter("@MONTHS5", MONTHS5);
        m_db.AddParameter("@MONTHS6", MONTHS6);
        m_db.AddParameter("@MONTHS7", MONTHS7);
        m_db.AddParameter("@MONTHS8", MONTHS8);
        m_db.AddParameter("@MONTHS9", MONTHS9);
        m_db.AddParameter("@MONTHS10", MONTHS10);
        m_db.AddParameter("@MONTHS11", MONTHS11);
        m_db.AddParameter("@MONTHS12", MONTHS12);



        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DELTBSALESSCHEDULES(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE [TKBUSINESS].[dbo].[TBSALESSCHEDULES] WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

    
    #endregion


}
