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


public partial class CDS_WebPage_TKRESEARCHTBDEVNEWDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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
                SEARCHTTBDEVNEW(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE(lblParam.Text);

        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE(lblParam.Text);

        SEARCHTTBDEVNEW(lblParam.Text);
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

    public void SEARCHTTBDEVNEW(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                       SELECT 
                        [SERNO]
                        ,[STATUS]
                        ,CONVERT(NVARCHAR(10),[SDATES],111) AS SDATES
                        ,[PRODUCTS]
                        ,[CLIENTS]
                        ,[SALES]
                        ,[NUMS]
                        ,CONVERT(NVARCHAR(10),[TESTDATES],111) AS TESTDATES
                        ,[TESTMEMO]
                        FROM [TKRESEARCH].[dbo].[TBDEVNEW]
                        WHERE [SERNO]=@SERNO 
                        ";


        m_db.AddParameter("@SERNO", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.Text = dt.Rows[0]["STATUS"].ToString();
            RadDatePicker1.SelectedDate = Convert.ToDateTime(dt.Rows[0]["SDATES"].ToString());

            TextBox1.Text = dt.Rows[0]["PRODUCTS"].ToString();
            TextBox2.Text = dt.Rows[0]["CLIENTS"].ToString();
            TextBox3.Text = dt.Rows[0]["SALES"].ToString();
            TextBox4.Text = dt.Rows[0]["NUMS"].ToString();           
            RadDatePicker2.SelectedDate = Convert.ToDateTime(dt.Rows[0]["TESTDATES"].ToString());
            TextBox5.Text = dt.Rows[0]["TESTMEMO"].ToString();



        }




    }

    public void UPDATE(string ID)
    {
        //string ID = "";
        string SERNO = ID;
        string STATUS = DropDownList1.SelectedValue.ToString().Trim();
        string SDATES = RadDatePicker1.SelectedDate.Value.ToString("yyy/MM/dd");
        //string SDATES = TDATES1.Text.ToString().Trim();
        string PRODUCTS = TextBox1.Text.ToString().Trim();
        string CLIENTS = TextBox2.Text.ToString().Trim();
        string SALES = TextBox3.Text.ToString().Trim();
        string NUMS = TextBox4.Text.ToString().Trim();
        string TESTDATES = RadDatePicker2.SelectedDate.Value.ToString("yyy/MM/dd");
        //string TESTDATES = TDATES2.Text.ToString().Trim();
        string TESTMEMO = TextBox5.Text.ToString().Trim();


        if (!string.IsNullOrEmpty(STATUS) && !string.IsNullOrEmpty(PRODUCTS))
        {
            UPDATETBDEVNEW(SERNO, STATUS, SDATES, PRODUCTS, CLIENTS, SALES, NUMS, TESTDATES, TESTMEMO);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBDEVNEW(string SERNO, string STATUS, string SDATES, string PRODUCTS, string CLIENTS, string SALES, string NUMS, string TESTDATES, string TESTMEMO)
    {

        if (string.IsNullOrEmpty(SDATES))
        {
            SDATES = "1911/1/1";
        }
        if (string.IsNullOrEmpty(TESTDATES))
        {
            TESTDATES = "1911/1/1";
        }


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKRESEARCH].[dbo].[TBDEVNEW]
                        SET [STATUS]=@STATUS
                        ,[SDATES]=@SDATES
                        ,[PRODUCTS]=@PRODUCTS
                        ,[CLIENTS]=@CLIENTS
                        ,[SALES]=@CLIENTS
                        ,[NUMS]=@SALES
                        ,[TESTDATES]=@TESTDATES
                        ,[TESTMEMO]=@TESTMEMO
                        WHERE[SERNO]=@SERNO
                            ";

        m_db.AddParameter("@SERNO", SERNO);
        m_db.AddParameter("@STATUS", STATUS);
        m_db.AddParameter("@SDATES", SDATES);
        m_db.AddParameter("@PRODUCTS", PRODUCTS);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@SALES", SALES);
        m_db.AddParameter("@NUMS", NUMS);
        m_db.AddParameter("@TESTDATES", TESTDATES);
        m_db.AddParameter("@TESTMEMO", TESTMEMO);

        m_db.ExecuteNonQuery(cmdTxt);


    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBDEVNEW(lblParam.Text);
    }

    public void DELTBDEVNEW(string ID)
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"  DELETE [TKRESEARCH].[dbo].[TBDEVNEW]  WHERE [SERNO]=@ID
        //                    ";

        //m_db.AddParameter("@ID", ID);

        //m_db.ExecuteNonQuery(cmdTxt);


        //Dialog.SetReturnValue2("NeedPostBack");
        //Dialog.Close(this);
    }
    #endregion


}
