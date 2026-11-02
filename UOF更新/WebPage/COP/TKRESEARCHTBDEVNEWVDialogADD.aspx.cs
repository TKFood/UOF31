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


public partial class CDS_WebPage_TKRESEARCHTBDEVNEWVDialogADD : Ede.Uof.Utility.Page.BasePage
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

    public bool IsDate(string strDate)
    {
        try
        {
            DateTime.Parse(strDate);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void ADD()
    {
        string ID = "";
        string SERNO = "";
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
            ADDTBDEVNEW(SERNO, STATUS, SDATES, PRODUCTS, CLIENTS, SALES, NUMS, TESTDATES, TESTMEMO);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }


    public void ADDTBDEVNEW(string SERNO,string STATUS, string SDATES, string PRODUCTS, string CLIENTS, string SALES, string NUMS, string TESTDATES, string TESTMEMO)
    {
        //SDATES是空的或不是日期
        if (string.IsNullOrEmpty(SDATES)||!IsDate(SDATES))
        {
            SDATES = "1911/1/1";
        }

        if (string.IsNullOrEmpty(TESTDATES) || !IsDate(TESTDATES))
        {
            TESTDATES = "1911/1/1";
        }

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

      
        string cmdTxt = @" 
                        INSERT INTO [TKRESEARCH].[dbo].[TBDEVNEW]
                        (STATUS,SDATES,PRODUCTS,CLIENTS,SALES,NUMS,TESTDATES,TESTMEMO)
                        VALUES
                        (@STATUS,@SDATES,@PRODUCTS,@CLIENTS,@SALES,@NUMS,@TESTDATES,@TESTMEMO)
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
    #endregion


}
