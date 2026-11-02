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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CDS_WebPage_TBBU_TBPROMOTIONNFEEDialogADD : Ede.Uof.Utility.Page.BasePage
{   

    protected void Page_Load(object sender, EventArgs e)
    {
        string ACCOUNT = null;
        string NAME = null;
        string DEPNAME = null;
        string JOB = null;
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        DEPNAME= Current.User.GroupName;
        JOB = Current.User.JobTitle;

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
            BindDropDownList2();
            BindDropDownList3();

            TextBox1.Text = DateTime.Now.Year.ToString();
            TextBox14.Text = DEPNAME;
            TextBox15.Text = JOB;
            TextBox2.Text = NAME;

            ////接收主頁面傳遞之參數
            //lblParam.Text = Request["ID"];


            //if (!string.IsNullOrEmpty(lblParam.Text))
            //{
            //   // SEARCHTCOPCONDTIONS(lblParam.Text);
            //}

        }
        
    }

    #region FUNCTION
    private void BindDropDownList()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("ID", typeof(String));
        //dt.Columns.Add("KIND", typeof(String));

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"SELECT [ID],[KIND] FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEKINDS] ORDER BY [ID] ";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    DropDownList1.DataSource = dt;
        //    DropDownList1.DataTextField = "KIND";
        //    DropDownList1.DataValueField = "KIND";
        //    DropDownList1.DataBind();

        //}
        //else
        //{

        //}
    }

    private void BindDropDownList2()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("ID", typeof(String));
        //dt.Columns.Add("METHOD", typeof(String));

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"SELECT  [ID],[METHOD] FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEMETHODS] ORDER BY [ID]";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    DropDownList2.DataSource = dt;
        //    DropDownList2.DataTextField = "METHOD";
        //    DropDownList2.DataValueField = "METHOD";
        //    DropDownList2.DataBind();

        //}
        //else
        //{

        //}
    }

    private void BindDropDownList3()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("ID", typeof(String));
        //dt.Columns.Add("METHODWAY", typeof(String));

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"SELECT [ID],[METHODWAY] FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEMETHODWAYS] ORDER BY [ID]";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    DropDownList3.DataSource = dt;
        //    DropDownList3.DataTextField = "METHODWAY";
        //    DropDownList3.DataValueField = "METHODWAY";
        //    DropDownList3.DataBind();

        //}
        //else
        //{

        //}
    }

    #endregion


    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗       

        ADD();

        Dialog.SetReturnValue2("ADD");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        Dialog.SetReturnValue2("ADD");
        ADD();

       
    }

    #endregion

    #region FUNCTION
   

    public void ADD()
    { 
        string YEARS = TextBox1.Text;
        string SALES = TextBox2.Text;
        string NAMES = TextBox3.Text;
        //string KINDS = DropDownList1.Text;
        //string PROMOTIONS = DropDownList2.Text;
        //string PROMOTIONSSETS = DropDownList3.Text;
        string KINDS = "";
        string PROMOTIONS = "";
        string PROMOTIONSSETS = "";
        string SDATES = TextBox7.Text;
        string CLIENTS = TextBox8.Text;
        string STORES = TextBox9.Text;
        //string SALESNUMS = TextBox10.Text;
        string SALESNUMS = "0";
        string SALESMONEYS = TextBox11.Text;
        string PROFITS = TextBox12.Text;
        string COSTMONEYS = TextBox4.Text;
        string FEEMONEYS = TextBox5.Text;

        //string COMMENTS = TextBox13.Text;
        string COMMENTS ="";
        string DEPNAME = TextBox14.Text;
        string TITLES = TextBox15.Text;
        string ACTIONS = TextBox6.Text;
        string PRODUCTS= TextBox10.Text;



        if ( !string.IsNullOrEmpty(YEARS)&& !string.IsNullOrEmpty(NAMES))
        {

            ADDTBPROMOTIONNFEE(YEARS, SALES, NAMES, KINDS, PROMOTIONS, PROMOTIONSSETS, SDATES, CLIENTS, STORES, SALESNUMS, SALESMONEYS, COSTMONEYS, FEEMONEYS, PROFITS, COMMENTS, DEPNAME, TITLES, ACTIONS, PRODUCTS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDTBPROMOTIONNFEE(string YEARS, string SALES, string NAMES, string KINDS, string PROMOTIONS, string PROMOTIONSSETS, string SDATES, string CLIENTS, string STORES, string SALESNUMS, string SALESMONEYS, string COSTMONEYS, string FEEMONEYS, string PROFITS, string COMMENTS, string DEPNAME, string TITLES,string ACTIONS,string PRODUCTS)
    {
        Label8.Text = "";

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        try
        {
            string cmdTxt = @"  
                            INSERT INTO  [TKBUSINESS].[dbo].[TBPROMOTIONNFEE]
                            ([YEARS],[DEPNAME],[TITLES],[SALES],[NAMES],[KINDS],[PROMOTIONS],[PROMOTIONSSETS],[SDATES],[CLIENTS],[STORES],[SALESNUMS],[SALESMONEYS], [COSTMONEYS], [FEEMONEYS],[PROFITS],[COMMENTS],[ACTIONS],[PRODUCTS])
                            VALUES
                            (@YEARS,@DEPNAME,@TITLES, @SALES, @NAMES, @KINDS, @PROMOTIONS, @PROMOTIONSSETS, @SDATES, @CLIENTS, @STORES, @SALESNUMS, @SALESMONEYS,@COSTMONEYS,@FEEMONEYS, @PROFITS, @COMMENTS,@ACTIONS,@PRODUCTS)

                            ";



            m_db.AddParameter("@YEARS", YEARS);
            m_db.AddParameter("@DEPNAME", DEPNAME);
            m_db.AddParameter("@TITLES", TITLES);
            m_db.AddParameter("@SALES", SALES);
            m_db.AddParameter("@NAMES", NAMES);
            m_db.AddParameter("@KINDS", KINDS);
            m_db.AddParameter("@PROMOTIONS", PROMOTIONS);
            m_db.AddParameter("@PROMOTIONSSETS", PROMOTIONSSETS);
            m_db.AddParameter("@SDATES", SDATES);
            m_db.AddParameter("@CLIENTS", CLIENTS);
            m_db.AddParameter("@STORES", STORES);
            m_db.AddParameter("@SALESNUMS", SALESNUMS);
            m_db.AddParameter("@SALESMONEYS", SALESMONEYS);
            m_db.AddParameter("@COSTMONEYS", COSTMONEYS);
            m_db.AddParameter("@FEEMONEYS", FEEMONEYS);
            m_db.AddParameter("@PROFITS", PROFITS);
            m_db.AddParameter("@COMMENTS", COMMENTS);
            m_db.AddParameter("@ACTIONS", ACTIONS);
            m_db.AddParameter("@PRODUCTS", PRODUCTS);




            m_db.ExecuteNonQuery(cmdTxt);

            Label8.Text = "成功";
        }
        catch
        {
            Label8.Text = "新增失敗";
        }
        finally
        {

        }

        

    }

   

    #endregion


}
