using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CDS_WebPage_TKRESEARCHTBSALESDEVMEMODialogADD : Ede.Uof.Utility.Page.BasePage
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
        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();
    }

    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();
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

       
    }

    public void ADD()
    {
        string ID = "";
        string SERNO = "";
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
        string TESTDATES = RadDatePicker3.SelectedDate.Value.ToString("yyyy/MM/dd");
        string SALESCHECKOK = TextBox12.Text.ToString().Trim();
        string OWNER = TextBox13.Text.ToString().Trim();
        string MEMO = TextBox14.Text.ToString().Trim();
        string DEVMEMO = TextBox15.Text.ToString().Trim();

        if (!string.IsNullOrEmpty(STATUS) && !string.IsNullOrEmpty(CLIENT) && !string.IsNullOrEmpty(PROD))
        {
            ADDTBSALESDEVMEMO(ID, SERNO, STATUS, CLIENT, PROD, PRICES, PROMOTIONS, SPEC, VALID, PLACES, ONSALES, ONSALESDATES, PRODESGIN, ASSESSMENTDATES, COSTSDATES, SALESPRICES, TEST, TESTDATES, OWNER, MEMO,DEVMEMO, SALESCHECKOK);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDTBSALESDEVMEMO(string ID, string SERNO, string STATUS, string CLIENT, string PROD, string PRICES, string PROMOTIONS, string SPEC, string VALID, string PLACES, string ONSALES,string ONSALESDATES, string PRODESGIN, string ASSESSMENTDATES, string COSTSDATES, string SALESPRICES, string TEST, string TESTDATES, string OWNER, string MEMO,string DEVMEMO,string SALESCHECKOK)
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
        if(string.IsNullOrEmpty(ONSALESDATES))
        {
            ONSALESDATES = "1911/1/1";
        }
        if (string.IsNullOrEmpty(SALESCHECKOK))
        {
            SALESCHECKOK = "1911/1/1";
        }

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  INSERT INTO [TKRESEARCH].[dbo].[TBSALESDEVMEMO]
                            ([STATUS],[CLIENT],[PROD],[PRICES],[PROMOTIONS],[SPEC],[VALID],[PLACES],[ONSALES],[ONSALESDATES],[PRODESGIN],[ASSESSMENTDATES],[COSTSDATES],[SALESPRICES],[TEST],[TESTDATES],[OWNER],[MEMO],[DEVMEMO],[SALESCHECKOK])
                            VALUES
                            (@STATUS,@CLIENT,@PROD,@PRICES,@PROMOTIONS,@SPEC,@VALID,@PLACES,@ONSALES,@ONSALESDATES,@PRODESGIN,@ASSESSMENTDATES,@COSTSDATES,@SALESPRICES,@TEST,@TESTDATES,@OWNER,@MEMO,@DEVMEMO,@SALESCHECKOK)
                            ";


    
        m_db.AddParameter("@STATUS", STATUS);
        m_db.AddParameter("@CLIENT", CLIENT);
        m_db.AddParameter("@PROD", PROD);
        m_db.AddParameter("@PRICES", PRICES);
        m_db.AddParameter("@PROMOTIONS", PROMOTIONS);
        m_db.AddParameter("@SPEC", SPEC);
        m_db.AddParameter("@VALID", VALID);
        m_db.AddParameter("@PLACES", PLACES);
        m_db.AddParameter("@ONSALES", ONSALES);
        m_db.AddParameter("@ONSALESDATES", Convert.ToDateTime(ONSALESDATES));
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



        


    }
    #endregion


}
