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


public partial class CDS_WebPage_TKRESEARCHTBSALESDEVDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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
                SEARCHTTBSALESDEV(lblParam.Text);
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

        SEARCHTTBSALESDEV(lblParam.Text);
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

    public void SEARCHTTBSALESDEV(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT  [SERNO]
                        ,[STATUS]
                        ,[CLIENT]
                        ,[SALES]
                        ,[PROD]
                        ,[SPEC]
                        ,[VALID]
                        ,[PRICES]
                        ,[PROMOTIONS]
                        ,[PLACES]
                        ,[ONSALES]
                        ,CONVERT(NVARCHAR,[ONSALESDATES],111) AS ONSALESDATES
                        ,[DESIGNS]
                        ,[SALESHISTORYS]
                        ,[DEVHISTORYS]
                        FROM [TKRESEARCH].[dbo].[TBSALESDEV]
                        WHERE [SERNO]=@SERNO 
                        ";


        m_db.AddParameter("@SERNO", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.Text = dt.Rows[0]["STATUS"].ToString();
            TextBox1.Text = dt.Rows[0]["CLIENT"].ToString();
            TextBox2.Text = dt.Rows[0]["SALES"].ToString();
            TextBox3.Text = dt.Rows[0]["PROD"].ToString();
            TextBox4.Text = dt.Rows[0]["SPEC"].ToString();
            TextBox5.Text = dt.Rows[0]["VALID"].ToString();
            TextBox6.Text = dt.Rows[0]["PRICES"].ToString();
            TextBox7.Text = dt.Rows[0]["PROMOTIONS"].ToString();
            TextBox8.Text = dt.Rows[0]["PLACES"].ToString();
            TextBox9.Text = dt.Rows[0]["ONSALES"].ToString();
            RadDatePicker1.SelectedDate = Convert.ToDateTime(dt.Rows[0]["ONSALESDATES"].ToString());           
            TextBox10.Text = dt.Rows[0]["DESIGNS"].ToString();
            TextBox11.Text = dt.Rows[0]["SALESHISTORYS"].ToString();
            TextBox12.Text = dt.Rows[0]["DEVHISTORYS"].ToString();


        }

       


    }

    public void UPDATE()
    {
        string SERNO = lblParam.Text;
        string STATUS = DropDownList1.SelectedValue.ToString().Trim();
        string CLIENT = TextBox1.Text.ToString().Trim();
        string SALES = TextBox2.Text.ToString().Trim();
        string PROD = TextBox3.Text.ToString().Trim();
        string SPEC = TextBox4.Text.ToString().Trim();
        string VALID = TextBox5.Text.ToString().Trim();
        string PRICES = TextBox6.Text.ToString().Trim();
        string PROMOTIONS = TextBox7.Text.ToString().Trim();
        string PLACES = TextBox8.Text.ToString().Trim();
        string ONSALES = TextBox9.Text.ToString().Trim();
        string ONSALESDATES = RadDatePicker1.SelectedDate.Value.ToString("yyyy/MM/dd");
        string DESIGNS = TextBox10.Text.ToString().Trim();
        string SALESHISTORYS = TextBox11.Text.ToString().Trim();
        string DEVHISTORYS = TextBox12.Text.ToString().Trim();


        if (!string.IsNullOrEmpty(SERNO) && !string.IsNullOrEmpty(STATUS) && !string.IsNullOrEmpty(CLIENT) && !string.IsNullOrEmpty(PROD))
        {
            UPDATETBSALESDEV(SERNO, STATUS, CLIENT, SALES, PROD, SPEC, VALID, PRICES, PROMOTIONS, PLACES, ONSALES, ONSALESDATES, DESIGNS, SALESHISTORYS,  DEVHISTORYS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBSALESDEV(string SERNO,string STATUS, string CLIENT, string SALES, string PROD, string SPEC, string VALID, string PRICES, string PROMOTIONS, string PLACES, string ONSALES, string ONSALESDATES, string DESIGNS, string SALESHISTORYS, string DEVHISTORYS)
    {

        if (string.IsNullOrEmpty(ONSALESDATES))
        {
            ONSALESDATES = "1911/1/1";
        }
    

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKRESEARCH].[dbo].[TBSALESDEV]
                        SET [STATUS]=@STATUS
                        ,[CLIENT]=@CLIENT
                        ,[SALES]=@SALES
                        ,[PROD]=@PROD
                        ,[SPEC]=@SPEC
                        ,[VALID]=@VALID
                        ,[PRICES]=@PRICES
                        ,[PROMOTIONS]=@PROMOTIONS
                        ,[PLACES]=@PLACES
                        ,[ONSALES]=@ONSALES
                        ,[ONSALESDATES]=@ONSALESDATES
                        ,[DESIGNS]=@DESIGNS
                        ,[SALESHISTORYS]=@SALESHISTORYS
                        ,[DEVHISTORYS]=@DEVHISTORYS
                        WHERE[SERNO]=@SERNO
                            ";






        m_db.AddParameter("@SERNO", SERNO);
        m_db.AddParameter("@STATUS", STATUS);
        m_db.AddParameter("@CLIENT", CLIENT);
        m_db.AddParameter("@SALES", SALES);
        m_db.AddParameter("@PROD", PROD);
        m_db.AddParameter("@SPEC", SPEC);
        m_db.AddParameter("@VALID", VALID);
        m_db.AddParameter("@PRICES", PRICES);
        m_db.AddParameter("@PROMOTIONS", PROMOTIONS);
        m_db.AddParameter("@PLACES", PLACES);
        m_db.AddParameter("@ONSALES", ONSALES);
        m_db.AddParameter("@ONSALESDATES", Convert.ToDateTime(ONSALESDATES));
        m_db.AddParameter("@DESIGNS", DESIGNS);
        m_db.AddParameter("@SALESHISTORYS", SALESHISTORYS);
        m_db.AddParameter("@DEVHISTORYS", DEVHISTORYS);

        m_db.ExecuteNonQuery(cmdTxt);


    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBSALESDEV(lblParam.Text);
    }

    public void DELTBSALESDEV(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE [TKRESEARCH].[dbo].[TBSALESDEV]  WHERE [SERNO]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    #endregion


}
