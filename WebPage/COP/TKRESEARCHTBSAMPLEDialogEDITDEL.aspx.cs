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


public partial class CDS_WebPage_TKRESEARCHTBSAMPLEDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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
            //接收主頁面傳遞之參數
            lblParam.Text = Request["ID"];


            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHTKRESEARCHTBSAMPLE(lblParam.Text);
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

        SEARCHTKRESEARCHTBSAMPLE(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTKRESEARCHTBSAMPLE(lblParam.Text);
    }


   

    #endregion

    #region FUNCTION
    private void BindDropDownList()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("SALESFOCUS", typeof(String));


        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"SELECT  [ID],[STORES] FROM [TKBUSINESS].[dbo].[STORESKIND] ORDER BY ID";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    DropDownList1.DataSource = dt;
        //    DropDownList1.DataTextField = "STORES";
        //    DropDownList1.DataValueField = "STORES";
        //    DropDownList1.DataBind();

        //}
        //else
        //{

        //}



    }

    public void SEARCHTKRESEARCHTBSAMPLE(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                        SELECT 
                        [ID]
                        ,[FORMID],[DV01],[DV02],[DV03],[DV04],[DV05],[DV06],[DV07],[DV08],[DV09],[DV10]
                        ,[DVV01],[DVV02],[DVV03],[DVV04],[DVV05],[DVV06],[DVV07],[DVV08]
                        ,[ISCLOSE]
                        ,REPLACE([COMMENTS],char(10),'<br/>') AS [COMMENTS] 
                        ,CONVERT(NVARCHAR, [UPDATEDATES],111) AS UPDATEDATES 
                        ,CONVERT(NVARCHAR, [FORMDATES],111) AS FORMDATES 
                        ,CONVERT(NVARCHAR, [PURDATES],111) AS PURDATES 

                        FROM [TKRESEARCH].[dbo].[TBSAMPLE]
                        WHERE [ID]=@ID
                        ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["ID"].ToString();
            TextBox2.Text = dt.Rows[0]["FORMID"].ToString();
            TextBox3.Text = dt.Rows[0]["DV01"].ToString();
            TextBox4.Text = dt.Rows[0]["DV02"].ToString();
            TextBox5.Text = dt.Rows[0]["DV03"].ToString();
            TextBox6.Text = dt.Rows[0]["DV05"].ToString();
            TextBox7.Text = dt.Rows[0]["DV06"].ToString();
            TextBox8.Text = dt.Rows[0]["DV07"].ToString();
            TextBox9.Text = dt.Rows[0]["DV08"].ToString();
            TextBox10.Text = dt.Rows[0]["DV09"].ToString();
            TextBox11.Text = dt.Rows[0]["DV10"].ToString();
            TextBox12.Text = dt.Rows[0]["DVV01"].ToString();
            TextBox13.Text = dt.Rows[0]["DVV02"].ToString();
            TextBox14.Text = dt.Rows[0]["DVV03"].ToString();
            TextBox15.Text = dt.Rows[0]["DVV04"].ToString();
            TextBox16.Text = dt.Rows[0]["DVV05"].ToString();
            TextBox17.Text = dt.Rows[0]["DVV06"].ToString();
            TextBox18.Text = dt.Rows[0]["DVV07"].ToString();
            TextBox19.Text = dt.Rows[0]["DVV08"].ToString();
            TextBox20.Text = dt.Rows[0]["ISCLOSE"].ToString();
            TextBox21.Text = dt.Rows[0]["COMMENTS"].ToString();
            TextBox22.Text = dt.Rows[0]["UPDATEDATES"].ToString();
            TextBox23.Text = dt.Rows[0]["FORMDATES"].ToString();

            if(!string.IsNullOrEmpty(dt.Rows[0]["PURDATES"].ToString()))
            {
                RadDatePicker1.SelectedDate = Convert.ToDateTime(dt.Rows[0]["PURDATES"].ToString());
            }
            else
            {

            }
           


        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string FORMID = TextBox2.Text.Trim();
        string DV01 = TextBox3.Text.Trim();
        string DV02 = TextBox4.Text.Trim();
        string DV03 = TextBox5.Text.Trim();
        string DV05 = TextBox6.Text.Trim();
        string DV06 = TextBox7.Text.Trim();
        string DV07 = TextBox8.Text.Trim();
        string DV08 = TextBox9.Text.Trim();
        string DV09 = TextBox10.Text.Trim();
        string DV10 = TextBox11.Text.Trim();
        string DVV01 = TextBox12.Text.Trim();
        string DVV02 = TextBox13.Text.Trim();
        string DVV03 = TextBox14.Text.Trim();
        string DVV04 = TextBox15.Text.Trim();
        string DVV05 = TextBox16.Text.Trim();
        string DVV06 = TextBox17.Text.Trim();
        string DVV07 = TextBox18.Text.Trim();
        string DVV08 = TextBox19.Text.Trim();
        string ISCLOSE = TextBox20.Text.Trim();
        string COMMENTS = TextBox21.Text.Trim();
        string UPDATEDATES = TextBox22.Text.Trim();
        string FORMDATES = TextBox23.Text.Trim();
        string PURDATES = RadDatePicker1.SelectedDate.Value.ToString("yyyy/MM/dd");



        if (!string.IsNullOrEmpty(ID) )
        {
            UPDATETBPROJECTS(ID, FORMID, DV01, DV02, DV03, DV05, DV06, DV07, DV08, DV09, DV10, DVV01, DVV02, DVV03, DVV04, DVV05, DVV06, DVV07, DVV08, ISCLOSE, COMMENTS, UPDATEDATES, FORMDATES, PURDATES);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBPROJECTS(string ID, string FORMID, string DV01, string DV02, string DV03, string DV05, string DV06, string DV07, string DV08, string DV09, string DV10, string DVV01, string DVV02, string DVV03, string DVV04, string DVV05, string DVV06, string DVV07, string DVV08, string ISCLOSE, string COMMENTS, string UPDATEDATES, string FORMDATES, string PURDATES)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                            UPDATE [TKRESEARCH].[dbo].[TBSAMPLE] 
                            SET 
                            [FORMID]=@FORMID
                            ,[DV01]=@DV01
                            ,[DV02]=@DV02
                            ,[DV03]=@DV03
                            ,[DV05]=@DV05
                            ,[DV06]=@DV06
                            ,[DV07]=@DV07
                            ,[DV08]=@DV08
                            ,[DV09]=@DV09
                            ,[DV10]=@DV10
                            ,[DVV01]=@DVV01
                            ,[DVV02]=@DVV02
                            ,[DVV03]=@DVV03
                            ,[DVV04]=@DVV04
                            ,[DVV05]=@DVV05
                            ,[DVV06]=@DVV06
                            ,[DVV07]=@DVV07
                            ,[DVV08]=@DVV08
                            ,[ISCLOSE]=@ISCLOSE
                            ,[COMMENTS]=@COMMENTS
                            ,[UPDATEDATES]=@UPDATEDATES
                            ,[FORMDATES]=@FORMDATES
                            ,[PURDATES]=@PURDATES
                            WHERE [ID]=@ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@FORMID", FORMID);
        m_db.AddParameter("@DV01", DV01);
        m_db.AddParameter("@DV02", DV02);
        m_db.AddParameter("@DV03", DV03);
        m_db.AddParameter("@DV05", DV05);
        m_db.AddParameter("@DV06", DV06);
        m_db.AddParameter("@DV07", DV07);
        m_db.AddParameter("@DV08", DV08);
        m_db.AddParameter("@DV09", DV09);
        m_db.AddParameter("@DV10", DV10);
        m_db.AddParameter("@DVV01", DVV01);
        m_db.AddParameter("@DVV02", DVV02);
        m_db.AddParameter("@DVV03", DVV03);
        m_db.AddParameter("@DVV04", DVV04);
        m_db.AddParameter("@DVV05", DVV05);
        m_db.AddParameter("@DVV06", DVV06);
        m_db.AddParameter("@DVV07", DVV07);
        m_db.AddParameter("@DVV08", DVV08);
        m_db.AddParameter("@ISCLOSE", ISCLOSE);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@UPDATEDATES", UPDATEDATES);
        m_db.AddParameter("@FORMDATES", FORMDATES);
        m_db.AddParameter("@PURDATES", PURDATES);




        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DELTKRESEARCHTBSAMPLE(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"
                        DELETE [TKRESEARCH].[dbo].[TBSAMPLE]  WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

   
    #endregion


}
