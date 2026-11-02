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


public partial class CDS_WebPage_TBBU_COPCONDTIONSDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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
                SEARCHTCOPCONDTIONS(lblParam.Text);
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

        SEARCHTCOPCONDTIONS(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELCOPCONDTIONS(lblParam.Text);
    }


    #endregion

    #region FUNCTION


    public void SEARCHTCOPCONDTIONS(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                        SELECT 
                        [ID]
                        ,[SERNO]
                        ,[MA001]
                        ,[MA002]
                        ,[CONTACTPERSON]
                        ,[TEL1]
                        ,[TEL2]
                        ,[EMAILS]
                        ,[ISPURATTCH]
                        ,[ISCOPATTCH]
                        ,[ISSHOWMONEYS]
                        ,[ISINVOICES]
                        ,[ISSHIPMARK]
                        ,[LIMITDAYS]
                        ,[PAYMENT]
                        ,[SENDADDRESS]
                        ,[COMMENT]
                        FROM [TKBUSINESS].[dbo].[COPCONDTIONS]
                        WHERE [ID]=@ID
                        ORDER BY SERNO
                        ";
        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["MA001"].ToString();
            TextBox2.Text = dt.Rows[0]["MA002"].ToString();
            TextBox3.Text = dt.Rows[0]["CONTACTPERSON"].ToString();
            TextBox4.Text = dt.Rows[0]["TEL1"].ToString();
            TextBox5.Text = dt.Rows[0]["EMAILS"].ToString();
            TextBox6.Text = dt.Rows[0]["ISPURATTCH"].ToString();
            TextBox7.Text = dt.Rows[0]["ISCOPATTCH"].ToString();
            TextBox8.Text = dt.Rows[0]["ISSHOWMONEYS"].ToString();
            TextBox9.Text = dt.Rows[0]["ISINVOICES"].ToString();
            TextBox10.Text = dt.Rows[0]["ISSHIPMARK"].ToString();
            TextBox11.Text = dt.Rows[0]["LIMITDAYS"].ToString();
            TextBox12.Text = dt.Rows[0]["PAYMENT"].ToString();
            TextBox13.Text = dt.Rows[0]["SENDADDRESS"].ToString();
            TextBox14.Text = dt.Rows[0]["COMMENT"].ToString();




        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string MA001 = TextBox1.Text;
        string MA002 = TextBox2.Text;
        string CONTACTPERSON = TextBox3.Text;
        string TEL1 = TextBox4.Text;
        string EMAILS = TextBox5.Text;
        string ISPURATTCH = TextBox6.Text;
        string ISCOPATTCH = TextBox7.Text;
        string ISSHOWMONEYS = TextBox8.Text;
        string ISINVOICES = TextBox9.Text;
        string ISSHIPMARK = TextBox10.Text;
        string LIMITDAYS = TextBox11.Text;
        string PAYMENT = TextBox12.Text;
        string SENDADDRESS = TextBox13.Text;
        string COMMENT = TextBox14.Text;

        if (!string.IsNullOrEmpty(ID) )
        {
            UPDATETBCOPCONDTIONS(ID, MA001, MA002, CONTACTPERSON, TEL1, EMAILS,ISPURATTCH, ISCOPATTCH, ISSHOWMONEYS, ISINVOICES, ISSHIPMARK, LIMITDAYS, PAYMENT, SENDADDRESS, COMMENT);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBCOPCONDTIONS(string ID, string MA001, string MA002, string CONTACTPERSON, string TEL1, string EMAILS, string ISPURATTCH, string ISCOPATTCH, string ISSHOWMONEYS, string ISINVOICES, string ISSHIPMARK, string LIMITDAYS, string PAYMENT, string SENDADDRESS, string COMMENT)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKBUSINESS].[dbo].[COPCONDTIONS]
                        SET MA001=@MA001
                        ,MA002=@MA002
                        ,CONTACTPERSON=@CONTACTPERSON
                        ,TEL1=@TEL1
                        ,EMAILS=@EMAILS
                        ,ISPURATTCH=@ISPURATTCH
                        ,ISCOPATTCH=@ISCOPATTCH
                        ,ISSHOWMONEYS=@ISSHOWMONEYS
                        ,ISINVOICES=@ISINVOICES
                        ,ISSHIPMARK=@ISSHIPMARK
                        ,LIMITDAYS=@LIMITDAYS
                        ,PAYMENT=@PAYMENT
                        ,SENDADDRESS=@SENDADDRESS
                        ,COMMENT=@COMMENT
                        WHERE [ID]=@ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@MA001", MA001);
        m_db.AddParameter("@MA002", MA002);
        m_db.AddParameter("@CONTACTPERSON", CONTACTPERSON);
        m_db.AddParameter("@TEL1", TEL1);
        m_db.AddParameter("@EMAILS", EMAILS);
        m_db.AddParameter("@ISPURATTCH", ISPURATTCH);
        m_db.AddParameter("@ISCOPATTCH", ISCOPATTCH);
        m_db.AddParameter("@ISSHOWMONEYS", ISSHOWMONEYS);
        m_db.AddParameter("@ISINVOICES", ISINVOICES);
        m_db.AddParameter("@ISSHIPMARK", ISSHIPMARK);
        m_db.AddParameter("@LIMITDAYS", LIMITDAYS);
        m_db.AddParameter("@PAYMENT", PAYMENT);
        m_db.AddParameter("@SENDADDRESS", SENDADDRESS);
        m_db.AddParameter("@COMMENT", COMMENT);

        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DELTBSALESDEVMEMO(string SERNO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE [TKRESEARCH].[dbo].[TBPROJECT]  WHERE[SERNO]=@SERNO
                            ";

        m_db.AddParameter("@SERNO", SERNO);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

    public void DELCOPCONDTIONS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  UPDATE  [TKBUSINESS].[dbo].[COPCONDTIONS] SET [ISUSED]='N'  WHERE[ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);



        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    #endregion


}
