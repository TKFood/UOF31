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


public partial class CDS_WebPage_TBBU_TBBU_TBPROMOTIONNFEEDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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
            BindDropDownList2();
            BindDropDownList3();

            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHTBPROMOTIONNFEE(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        //Session["KIND"] = DropDownList1.Text;//賦值Session["KIND"]

        UPDATE();

        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE();
        SEARCHTBPROMOTIONNFEE(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBPROMOTIONNFEE(lblParam.Text);
        SEARCHTBPROMOTIONNFEE(lblParam.Text);
    }


    #endregion

    #region FUNCTION
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("VALUE", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[KINDS],[NAMES],[VALUE] FROM [TKBUSINESS].[dbo].[TBPARA] WHERE [KINDS]='是否結案EDIT' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "VALUE";
            DropDownList1.DataValueField = "VALUE";
            DropDownList1.DataBind();

        }
        else
        {

        }
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

    public void SEARCHTBPROMOTIONNFEE(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                        SELECT  
                        [YEARS],[DEPNAME],[TITLES],[SALES],[NAMES],[KINDS],[PROMOTIONS],[PROMOTIONSSETS],[SDATES],[CLIENTS],[STORES],[SALESNUMS],[SALESMONEYS],[COSTMONEYS],[FEEMONEYS],[PROFITS],[COMMENTS],[ACTSALESMONEYS]
                        ,[ACTCOSTMONEYS]
                        ,[ACTFEEMONEYS]
                        ,[ACTPROFITS]
                        ,[ACTIONS]
                        ,[PRODUCTS]
                        ,ISCLOSED
                          
                        FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEE]
                        WHERE [ID]=@ID
                        ";
        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            TextBox1.Text = dt.Rows[0]["YEARS"].ToString();
            TextBox2.Text = dt.Rows[0]["SALES"].ToString();
            TextBox3.Text = dt.Rows[0]["NAMES"].ToString();        
            TextBox7.Text = dt.Rows[0]["SDATES"].ToString();
            TextBox8.Text = dt.Rows[0]["CLIENTS"].ToString();
            TextBox9.Text = dt.Rows[0]["STORES"].ToString();          
            TextBox11.Text = dt.Rows[0]["SALESMONEYS"].ToString();
            TextBox12.Text = dt.Rows[0]["PROFITS"].ToString();
            TextBox13.Text = dt.Rows[0]["COMMENTS"].ToString();
            TextBox14.Text = dt.Rows[0]["DEPNAME"].ToString();
            TextBox15.Text = dt.Rows[0]["TITLES"].ToString();
            TextBox4.Text = dt.Rows[0]["COSTMONEYS"].ToString();
            TextBox5.Text = dt.Rows[0]["FEEMONEYS"].ToString();
            TextBox10.Text = dt.Rows[0]["ACTSALESMONEYS"].ToString();
            TextBox13.Text = dt.Rows[0]["ACTCOSTMONEYS"].ToString();
            TextBox16.Text = dt.Rows[0]["ACTFEEMONEYS"].ToString();
            TextBox17.Text = dt.Rows[0]["ACTPROFITS"].ToString();
            TextBox6.Text = dt.Rows[0]["ACTIONS"].ToString();
            TextBox18.Text = dt.Rows[0]["PRODUCTS"].ToString();
            DropDownList1.SelectedValue = dt.Rows[0]["ISCLOSED"].ToString();



        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;

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
        string COMMENTS = "";
        string DEPNAME = TextBox14.Text;
        string TITLES = TextBox15.Text;
        string ACTIONS = TextBox6.Text;
        string ACTSALESMONEYS = TextBox10.Text;
        string ACTCOSTMONEYS = TextBox13.Text;
        string ACTFEEMONEYS = TextBox16.Text;
        string ACTPROFITS = TextBox17.Text;
        string PRODUCTS= TextBox18.Text;
        string ISCLOSED = DropDownList1.SelectedValue.ToString();




        if (!string.IsNullOrEmpty(ID) )
        {       
            UPDATETBPROMOTIONNFEE(ID, YEARS, SALES, NAMES, KINDS, PROMOTIONS, PROMOTIONSSETS, SDATES, CLIENTS, STORES, SALESNUMS, SALESMONEYS, COSTMONEYS, FEEMONEYS, PROFITS, COMMENTS, DEPNAME, TITLES, ACTSALESMONEYS, ACTCOSTMONEYS, ACTFEEMONEYS, ACTPROFITS, ACTIONS, PRODUCTS, ISCLOSED);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBPROMOTIONNFEE(string ID, string YEARS, string SALES, string NAMES, string KINDS, string PROMOTIONS, string PROMOTIONSSETS, string SDATES, string CLIENTS, string STORES, string SALESNUMS, string SALESMONEYS, string COSTMONEYS, string FEEMONEYS, string PROFITS, string COMMENTS, string DEPNAME, string TITLES, string ACTSALESMONEYS, string ACTCOSTMONEYS, string ACTFEEMONEYS, string ACTPROFITS,string ACTIONS,string PRODUCTS,string ISCLOSED)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"                         
                        UPDATE [TKBUSINESS].[dbo].[TBPROMOTIONNFEE]
                        SET 
                        [YEARS]=@YEARS
                        ,[DEPNAME]=@DEPNAME
                        ,[TITLES]=@TITLES
                        ,[SALES]=@SALES
                        ,[NAMES]=@NAMES
						,[KINDS]=@KINDS
                        ,[PROMOTIONS]=@PROMOTIONS
                        ,[PROMOTIONSSETS]=@PROMOTIONSSETS
                        ,[SDATES]=@SDATES
                        ,[CLIENTS]=@CLIENTS
                        ,[STORES]=@STORES
                        ,[SALESNUMS]=@SALESNUMS
                        ,[SALESMONEYS]=@SALESMONEYS
                        ,[PROFITS]=@PROFITS
                        ,[COMMENTS]=@COMMENTS
                        ,[COSTMONEYS]=@COSTMONEYS
                        ,[FEEMONEYS]=@FEEMONEYS
                        ,[ACTSALESMONEYS]=@ACTSALESMONEYS
                        ,[ACTCOSTMONEYS]=@ACTCOSTMONEYS
                        ,[ACTFEEMONEYS]=@ACTFEEMONEYS
                        ,[ACTPROFITS]=@ACTPROFITS
                        ,[ACTIONS]=@ACTIONS
                        ,[PRODUCTS]=@PRODUCTS
                        ,[ISCLOSED]=@ISCLOSED
                        WHERE  [ID]= @ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
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
        m_db.AddParameter("@PROFITS", PROFITS);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@COSTMONEYS", COSTMONEYS);
        m_db.AddParameter("@FEEMONEYS", FEEMONEYS);
        m_db.AddParameter("@ACTSALESMONEYS", ACTSALESMONEYS);
        m_db.AddParameter("@ACTCOSTMONEYS", ACTCOSTMONEYS);
        m_db.AddParameter("@ACTFEEMONEYS", ACTFEEMONEYS);
        m_db.AddParameter("@ACTPROFITS", ACTPROFITS);
        m_db.AddParameter("@ACTIONS", ACTIONS);
        m_db.AddParameter("@PRODUCTS", PRODUCTS);
        m_db.AddParameter("@ISCLOSED", ISCLOSED);

        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DELTBPROMOTIONNFEE(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        // 在執行刪除前，彈出 JavaScript 的確認對話框
        string confirmScript = "return confirm('確定要刪除嗎？');";
        Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "ConfirmDelete", confirmScript);

        if (Page.IsValid)
        {
            string cmdTxt = @"DELETE [TKBUSINESS].[dbo].[TBPROMOTIONNFEE] WHERE [ID]=@ID";
            m_db.AddParameter("@ID", ID);
            m_db.ExecuteNonQuery(cmdTxt);

            Dialog.SetReturnValue2("NeedPostBack");
            Dialog.Close(this);
        }
    }

    
    #endregion


}
