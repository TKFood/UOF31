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


public partial class CDS_WebPage_TBBU_TBCOPTFCHECKDialogEDIT : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        ROLES = SEARCHROLES(ACCOUNT.Trim());

        SETBUTTON();     

        ACCOUNTLabel.Text = ACCOUNT + NAME;

        if (ROLES.Equals("ADMIN"))
        {
            Button1.Enabled = true;
            //Button2.Enabled = true;
            Button3.Enabled = true;
        }
     
        else if (ROLES.Equals("MOC"))
        {
            Button1.Enabled = true;
           // Button2.Enabled = false;
            Button3.Enabled = false;
        }
        else if (ROLES.Equals("PUR"))
        {
            Button1.Enabled = false;
            //Button2.Enabled = true;
            Button3.Enabled = false;
        }
        else if (ROLES.Equals("SLAES"))
        {
            Button1.Enabled = false;
            //Button2.Enabled = false;
            Button3.Enabled = true;
        }




        //不顯示子視窗的按鈕
        ((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;
        ((Master_DialogMasterPage)this.Master).Button3Text = string.Empty;


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
                SEARCHCOPTF(lblParam.Text);
            }

        }

    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        Dialog.SetReturnValue2("OK");
       
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        Dialog.SetReturnValue2("OK");

        Dialog.Close(this);
        //SEARCHCOPTD(lblParam.Text);
    }

    //生管
    protected void Button1_Click(object sender, EventArgs e)
    {
        TextBox11.Text= DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
        //Button1.Text = "OK";
        ADDTBCOPTFCHECK(
                        TextBox1.Text,
                        TextBox2.Text,
                        TextBox24.Text,
                        TextBox23.Text,                       

                        TextBox11.Text,
                        DropDownList1.SelectedValue,
                        TextBox13.Text,
                         "",
                         "",
                         "",
                        TextBox12.Text,
                        TextBox17.Text
                        );
    }
    //採購
    protected void Button2_Click(object sender, EventArgs e)
    {
        //TextBox14.Text = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
        //Button1.Text = "OK";
        ADDTBCOPTFCHECK(
                        TextBox1.Text,
                        TextBox2.Text,
                        TextBox24.Text,
                        TextBox23.Text,

                        TextBox11.Text,
                        DropDownList1.SelectedValue,
                        TextBox13.Text,
                        "",
                        "",
                         "",
                        TextBox12.Text,
                        TextBox17.Text
                        );
    }
    //業務
    protected void Button3_Click(object sender, EventArgs e)
    {
        //TextBox12.Text = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
        //Button1.Text = "OK";
        ADDTBCOPTFCHECK(
                        TextBox1.Text,
                        TextBox2.Text,
                        TextBox24.Text,
                        TextBox23.Text,

                        TextBox11.Text,
                        DropDownList1.SelectedValue,
                        TextBox13.Text,
                        "",
                        "",
                         "",
                        TextBox12.Text,
                        TextBox17.Text
                        );
    }

    #endregion

    #region FUNCTION
    public void SETBUTTON()
    {
        Button1.Enabled = false;
        //Button2.Enabled = false;
        Button3.Enabled = false;
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
    public void SEARCHCOPTF(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
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

                        FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                        LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
                        LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
                        WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                        AND 1=1
                        AND LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004))=@ID

                        ORDER BY TE001,TE002,TE003,TF004
                        ";
        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["TF001"].ToString();
            TextBox2.Text = dt.Rows[0]["TF002"].ToString();
            TextBox3.Text = dt.Rows[0]["TF104"].ToString();
            TextBox4.Text = dt.Rows[0]["TF005"].ToString();
            TextBox5.Text = dt.Rows[0]["TF006"].ToString();
            TextBox6.Text = Convert.ToDecimal(dt.Rows[0]["TF009"].ToString()).ToString("N0");
            TextBox7.Text = dt.Rows[0]["TF010"].ToString();
            TextBox8.Text = Convert.ToDecimal(dt.Rows[0]["TF013"].ToString()).ToString("N2");
            TextBox9.Text = Convert.ToDecimal(dt.Rows[0]["TF014"].ToString()).ToString("N0");
            TextBox10.Text = dt.Rows[0]["TF015"].ToString();
            TextBox11.Text = dt.Rows[0]["MOCCHECKDATES"].ToString();
            //TextBox11.Text = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            //TextBox12.Text = dt.Rows[0]["MOCCHECKS"].ToString();
            DropDownList1.SelectedValue = dt.Rows[0]["MOCCHECKS"].ToString();
            TextBox13.Text = dt.Rows[0]["MOCCHECKSCOMMENTS"].ToString();
            //TextBox14.Text = dt.Rows[0]["PURCHECKDATES"].ToString();
            //TextBox14.Text = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            //TextBox15.Text = dt.Rows[0]["PURCHECKS"].ToString();
            //DropDownList2.SelectedValue = dt.Rows[0]["PURCHECKS"].ToString();
            //TextBox16.Text = dt.Rows[0]["PURCHECKSCOMMENTS"].ToString();
            TextBox12.Text = dt.Rows[0]["SALESCHECKDATES"].ToString();
            //TextBox12.Text = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            TextBox17.Text = dt.Rows[0]["SALESCHECKSCOMMENTS"].ToString();
            TextBox15.Text = dt.Rows[0]["TC053"].ToString();
            TextBox18.Text = Convert.ToDecimal(dt.Rows[0]["TF020"].ToString()).ToString("N0");
            //TextBox19.Text = dt.Rows[0]["TD009"].ToString();
            //TextBox20.Text = dt.Rows[0]["TD025"].ToString();
            TextBox21.Text = dt.Rows[0]["TE050"].ToString();
            TextBox22.Text = dt.Rows[0]["TF032"].ToString();
            TextBox23.Text = dt.Rows[0]["TF004"].ToString();
            TextBox24.Text = dt.Rows[0]["TE003"].ToString();
        }




    }

   



    public void ADDTBCOPTFCHECK(string TF001,
                                string TF002,
                                string TF003,
                                string TF004,                               
                                string MOCCHECKDATES,
                                string MOCCHECKS,
                                string MOCCHECKSCOMMENTS,
                                string PURCHECKDATES,
                                string PURCHECKS,
                                string PURCHECKSCOMMENTS,
                                string SALESCHECKDATES,
                                string SALESCHECKSCOMMENTS)
    {        

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
                        ,@PURCHECKDATES
                        ,@PURCHECKS
                        ,@PURCHECKSCOMMENTS
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
        m_db.AddParameter("@MOCCHECKDATES", MOCCHECKDATES);
        m_db.AddParameter("@MOCCHECKS", MOCCHECKS);
        m_db.AddParameter("@MOCCHECKSCOMMENTS", MOCCHECKSCOMMENTS);
        m_db.AddParameter("@PURCHECKDATES", PURCHECKDATES);
        m_db.AddParameter("@PURCHECKS", PURCHECKS);
        m_db.AddParameter("@PURCHECKSCOMMENTS", PURCHECKSCOMMENTS);
        m_db.AddParameter("@SALESCHECKDATES", SALESCHECKDATES);
        m_db.AddParameter("@SALESCHECKSCOMMENTS", SALESCHECKSCOMMENTS);


        m_db.ExecuteNonQuery(cmdTxt);

    }


    #endregion




}
