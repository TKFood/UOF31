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


public partial class CDS_WebPage_TKRESEARCHTBSALESDEVMEMODialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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
                SEARCHTTBSALESDEVMEMO(lblParam.Text);
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

        SEARCHTTBSALESDEVMEMO(lblParam.Text);
    }

    #endregion

    #region FUNCTION
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='MEMOSTATUS' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "PARANAME";
            DropDownList1.DataValueField = "PARANAME";
            DropDownList1.DataBind();

        }
        else
        {

        }

        //DataSet ds = new DataSet();
        //DatabaseHelper DbQuery = new DatabaseHelper();
        //DataTable dt = new DataTable();
        //DataRow ndr = dt.NewRow();

        //dt.Columns.Add("PARAID", typeof(String));
        //dt.Columns.Add("PARANAME", typeof(String));

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //using (SqlConnection conn = new SqlConnection(connectionString))
        //{
        //    SqlCommand command = new SqlCommand(@" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='MEMOSTATUS' ORDER BY [PARAID]", conn);

        //    ds.Clear();

        //    SqlDataAdapter adapter = new SqlDataAdapter(command);
        //    conn.Open();

        //    adapter.Fill(ds, command.ToString());

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        DropDownList1.DataSource = ds.Tables[0];
        //        DropDownList1.DataTextField = "PARANAME";
        //        DropDownList1.DataValueField = "PARANAME";
        //        DropDownList1.DataBind();

        //    }
        //    else
        //    {

        //    }
        //}
    }

    public void SEARCHTTBSALESDEVMEMO(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[SERNO],[STATUS],[CLIENT],[PROD],[PRICES],[PROMOTIONS],[SPEC],[VALID],[PLACES],[ONSALES],[ONSALESDATES],[PRODESGIN],CONVERT(NVARCHAR,[ASSESSMENTDATES],111) ASSESSMENTDATES,CONVERT(NVARCHAR,[COSTSDATES],111) COSTSDATES,[SALESPRICES],[TEST],CONVERT(NVARCHAR,[TESTDATES],111) TESTDATES,[OWNER],[MEMO],[DEVMEMO],[SALESCHECKOK] FROM [TKRESEARCH].[dbo].[TBSALESDEVMEMO] WHERE [ID]=@ID ";
        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.Text = dt.Rows[0]["STATUS"].ToString();
            TextBox1.Text = dt.Rows[0]["CLIENT"].ToString();
            TextBox2.Text = dt.Rows[0]["PROD"].ToString();
            TextBox3.Text = dt.Rows[0]["PRICES"].ToString();
            TextBox4.Text = dt.Rows[0]["PROMOTIONS"].ToString();
            TextBox5.Text = dt.Rows[0]["SPEC"].ToString();
            TextBox6.Text = dt.Rows[0]["VALID"].ToString();
            TextBox7.Text = dt.Rows[0]["PLACES"].ToString();
            TextBox8.Text = dt.Rows[0]["ONSALES"].ToString();
            TextBox9.Text = dt.Rows[0]["PRODESGIN"].ToString();
            RadDatePicker1.SelectedDate = Convert.ToDateTime(dt.Rows[0]["ASSESSMENTDATES"].ToString());
            RadDatePicker2.SelectedDate = Convert.ToDateTime(dt.Rows[0]["COSTSDATES"].ToString());
            TextBox10.Text = dt.Rows[0]["SALESPRICES"].ToString();
            TextBox11.Text = dt.Rows[0]["TEST"].ToString();
            RadDatePicker3.SelectedDate = Convert.ToDateTime(dt.Rows[0]["TESTDATES"].ToString());
            TextBox12.Text = dt.Rows[0]["SALESCHECKOK"].ToString();
            TextBox13.Text = dt.Rows[0]["OWNER"].ToString();
            TextBox14.Text = dt.Rows[0]["MEMO"].ToString();
            TextBox15.Text = dt.Rows[0]["DEVMEMO"].ToString();
            RadDatePicker4.SelectedDate = Convert.ToDateTime(dt.Rows[0]["ONSALESDATES"].ToString());

        }

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //using (SqlConnection conn = new SqlConnection(connectionString))
        //{
        //    SqlCommand command = new SqlCommand(" SELECT [ID],[SERNO],[STATUS],[CLIENT],[PROD],[PRICES],[PROMOTIONS],[SPEC],[VALID],[PLACES],[ONSALES],[PRODESGIN],CONVERT(NVARCHAR,[ASSESSMENTDATES],111) ASSESSMENTDATES,CONVERT(NVARCHAR,[COSTSDATES],111) COSTSDATES,[SALESPRICES],[TEST],CONVERT(NVARCHAR,[TESTDATES],111) TESTDATES,[OWNER],[MEMO] FROM [TKRESEARCH].[dbo].[TBSALESDEVMEMO] WHERE [ID]=@ID", conn);
        //    command.Parameters.AddWithValue("@ID", ID);

        //    DataSet ds = new DataSet();
        //    SqlDataAdapter adapter = new SqlDataAdapter(command);
        //    conn.Open();

        //    adapter.Fill(ds, command.ToString());

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        DropDownList1.Text = ds.Tables[0].Rows[0]["STATUS"].ToString();
        //        TextBox1.Text= ds.Tables[0].Rows[0]["CLIENT"].ToString();
        //        TextBox2.Text = ds.Tables[0].Rows[0]["PROD"].ToString();
        //        TextBox3.Text = ds.Tables[0].Rows[0]["PRICES"].ToString();
        //        TextBox4.Text = ds.Tables[0].Rows[0]["PROMOTIONS"].ToString();
        //        TextBox5.Text = ds.Tables[0].Rows[0]["SPEC"].ToString();
        //        TextBox6.Text = ds.Tables[0].Rows[0]["VALID"].ToString();
        //        TextBox7.Text = ds.Tables[0].Rows[0]["PLACES"].ToString();
        //        TextBox8.Text = ds.Tables[0].Rows[0]["ONSALES"].ToString();
        //        TextBox9.Text = ds.Tables[0].Rows[0]["PRODESGIN"].ToString();
        //        txtDate1.Text = ds.Tables[0].Rows[0]["ASSESSMENTDATES"].ToString();
        //        txtDate2.Text = ds.Tables[0].Rows[0]["COSTSDATES"].ToString();
        //        TextBox10.Text = ds.Tables[0].Rows[0]["SALESPRICES"].ToString();
        //        TextBox11.Text = ds.Tables[0].Rows[0]["TEST"].ToString();
        //        txtDate3.Text = ds.Tables[0].Rows[0]["TESTDATES"].ToString();
        //        TextBox13.Text = ds.Tables[0].Rows[0]["OWNER"].ToString();
        //        TextBox14.Text = ds.Tables[0].Rows[0]["MEMO"].ToString();

        //    }
        //}


    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string STATUS = DropDownList1.SelectedValue.ToString().Trim();
        string CLIENT = TextBox1.Text.ToString().Trim();
        string PROD = TextBox2.Text.ToString().Trim();
        string PRICES = TextBox3.Text.ToString().Trim();
        string PROMOTIONS = TextBox4.Text.ToString().Trim();
        string SPEC = TextBox5.Text.ToString().Trim();
        string VALID = TextBox6.Text.ToString().Trim();
        string PLACES = TextBox7.Text.ToString().Trim();
        string ONSALES = TextBox8.Text.ToString().Trim();
        string ONSALESDATES = RadDatePicker4.SelectedDate.Value.ToString("yyyy/MM/dd");
        string PRODESGIN = TextBox9.Text.ToString().Trim();
        string ASSESSMENTDATES = RadDatePicker1.SelectedDate.Value.ToString("yyyy/MM/dd");
        string COSTSDATES = RadDatePicker2.SelectedDate.Value.ToString("yyyy/MM/dd");
        string SALESPRICES = TextBox10.Text.ToString().Trim();
        string TEST = TextBox11.Text.ToString().Trim();
        string SALESCHECKOK = TextBox12.Text.ToString().Trim();
        string TESTDATES = RadDatePicker3.SelectedDate.Value.ToString("yyyy/MM/dd");
        string OWNER = TextBox13.Text.ToString().Trim();
        string MEMO = TextBox14.Text.ToString().Trim();
        string DEVMEMO = TextBox15.Text.ToString().Trim();

        if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(STATUS) && !string.IsNullOrEmpty(CLIENT) && !string.IsNullOrEmpty(PROD))
        {
            UPDATETBSALESDEVMEMO(ID, STATUS, CLIENT, PROD, PRICES, PROMOTIONS, SPEC, VALID, PLACES, ONSALES, ONSALESDATES, PRODESGIN, ASSESSMENTDATES, COSTSDATES, SALESPRICES, TEST, TESTDATES, OWNER, MEMO, DEVMEMO, SALESCHECKOK);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBSALESDEVMEMO(string ID,string STATUS,string CLIENT, string PROD, string PRICES, string PROMOTIONS, string SPEC, string VALID, string PLACES, string ONSALES,string ONSALESDATES, string PRODESGIN, string ASSESSMENTDATES, string COSTSDATES, string SALESPRICES, string TEST, string TESTDATES,string OWNER, string MEMO, string DEVMEMO,string SALESCHECKOK)
    {
        if (string.IsNullOrEmpty(ASSESSMENTDATES))
        {
            ASSESSMENTDATES = "1911/1/1";   
        }
        if (string.IsNullOrEmpty(COSTSDATES))
        {
            COSTSDATES = "1911/1/1";
        }
        if (string.IsNullOrEmpty(TESTDATES))
        {
            TESTDATES = "1911/1/1";
        }
        if (string.IsNullOrEmpty(ONSALESDATES))
        {
            ONSALESDATES = "1911/1/1";
        }
        if (string.IsNullOrEmpty(SALESCHECKOK))
        {
            SALESCHECKOK = "1911/1/1";
        }

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  UPDATE [TKRESEARCH].[dbo].[TBSALESDEVMEMO] SET [STATUS]=@STATUS,[CLIENT]=@CLIENT,[PROD]=@PROD,[PRICES]=@PRICES,[PROMOTIONS]=@PROMOTIONS,[SPEC]=@SPEC,[VALID]=@VALID,[PLACES]=@PLACES,[ONSALES]=@ONSALES,[ONSALESDATES]=@ONSALESDATES,[PRODESGIN]=@PRODESGIN,[ASSESSMENTDATES]=@ASSESSMENTDATES,[COSTSDATES]=@COSTSDATES,[SALESPRICES]=@SALESPRICES,[TEST]=@TEST,[TESTDATES]=@TESTDATES,[OWNER]=@OWNER,[MEMO]=@MEMO,[DEVMEMO]=@DEVMEMO,[SALESCHECKOK]=@SALESCHECKOK WHERE[ID]=@ID
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@STATUS", STATUS);
        m_db.AddParameter("@CLIENT", CLIENT);
        m_db.AddParameter("@PROD", PROD);
        m_db.AddParameter("@PRICES", PRICES);
        m_db.AddParameter("@PROMOTIONS", PROMOTIONS);
        m_db.AddParameter("@SPEC", SPEC);
        m_db.AddParameter("@VALID", VALID);
        m_db.AddParameter("@PLACES", PLACES);
        m_db.AddParameter("@ONSALES", ONSALES);
        m_db.AddParameter("@ONSALESDATES", ONSALESDATES);
        m_db.AddParameter("@PRODESGIN", PRODESGIN);
        m_db.AddParameter("@ASSESSMENTDATES", Convert.ToDateTime(ASSESSMENTDATES));
        m_db.AddParameter("@COSTSDATES", Convert.ToDateTime(COSTSDATES));
        m_db.AddParameter("@SALESPRICES", SALESPRICES);
        m_db.AddParameter("@TEST", TEST);
        m_db.AddParameter("@TESTDATES", Convert.ToDateTime(TESTDATES));
        m_db.AddParameter("@OWNER", OWNER);
        m_db.AddParameter("@MEMO", MEMO);
        m_db.AddParameter("@DEVMEMO", DEVMEMO);
        m_db.AddParameter("@SALESCHECKOK", SALESCHECKOK);

        m_db.ExecuteNonQuery(cmdTxt);



        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //var SQL = "UPDATE [TKRESEARCH].[dbo].[TBSALESDEVMEMO] SET [STATUS]=@STATUS,[CLIENT]=@CLIENT,[PROD]=@PROD,[PRICES]=@PRICES,[PROMOTIONS]=@PROMOTIONS,[SPEC]=@SPEC,[VALID]=@VALID,[PLACES]=@PLACES,[ONSALES]=@ONSALES,[PRODESGIN]=@PRODESGIN,[ASSESSMENTDATES]=@ASSESSMENTDATES,[COSTSDATES]=@COSTSDATES,[SALESPRICES]=@SALESPRICES,[TEST]=@TEST,[TESTDATES]=@TESTDATES,[OWNER]=@OWNER,[MEMO]=@MEMO WHERE[ID]=@ID";

        //using (SqlConnection cnn = new SqlConnection(connectionString))
        //{
        //    using (SqlCommand cmd = new SqlCommand(SQL, cnn))
        //    {
        //        cmd.Parameters.AddWithValue("@ID", ID);
        //        cmd.Parameters.AddWithValue("@STATUS", STATUS);
        //        cmd.Parameters.AddWithValue("@CLIENT", CLIENT);
        //        cmd.Parameters.AddWithValue("@PROD", PROD);
        //        cmd.Parameters.AddWithValue("@PRICES", PRICES);
        //        cmd.Parameters.AddWithValue("@PROMOTIONS", PROMOTIONS);
        //        cmd.Parameters.AddWithValue("@SPEC", SPEC);
        //        cmd.Parameters.AddWithValue("@VALID", VALID);
        //        cmd.Parameters.AddWithValue("@PLACES", PLACES);
        //        cmd.Parameters.AddWithValue("@ONSALES", ONSALES);
        //        cmd.Parameters.AddWithValue("@PRODESGIN", PRODESGIN);
        //        cmd.Parameters.AddWithValue("@ASSESSMENTDATES", Convert.ToDateTime(ASSESSMENTDATES));
        //        cmd.Parameters.AddWithValue("@COSTSDATES", Convert.ToDateTime(COSTSDATES));
        //        cmd.Parameters.AddWithValue("@SALESPRICES", SALESPRICES);
        //        cmd.Parameters.AddWithValue("@TEST", TEST);
        //        cmd.Parameters.AddWithValue("@TESTDATES", Convert.ToDateTime(TESTDATES));
        //        cmd.Parameters.AddWithValue("@OWNER", OWNER);
        //        cmd.Parameters.AddWithValue("@MEMO", MEMO);


        //        cnn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //}
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBSALESDEVMEMO(lblParam.Text);
    }

    public void DELTBSALESDEVMEMO(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE [TKRESEARCH].[dbo].[TBSALESDEVMEMO]  WHERE[ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    #endregion


}
