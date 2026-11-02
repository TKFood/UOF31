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
        Dialog.SetReturnValue2("REFRESH");

        //不顯示子視窗的按鈕
        //((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;
        //((Master_DialogMasterPage)this.Master).Button3Text = string.Empty;

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
            BindDropDownList4();
            BindDropDownList5();
            BindDropDownList6();
            BindDropDownList7();
            BindDropDownList8();
            BindDropDownList9();
            BindDropDownList10();

            SETTEXT();

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        
        ADD();


        Dialog.SetReturnValue2("REFRESH");
        Dialog.Close(this);
    }

    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();


        Dialog.SetReturnValue2("REFRESH");
        Dialog.Close(this);
    }

    #endregion


    #region FUNCTION

    public void SETTEXT()
    {
        TextBox5.Text = DateTime.Now.AddDays(7).ToString("yyyy/MM/dd");
        TextBox3.Text = DateTime.Now.AddDays(3).ToString("yyyy/MM/dd");
        TextBox6.Text = DateTime.Now.AddDays(7).ToString("yyyy/MM/dd");
        TextBox7.Text = DateTime.Now.AddDays(5).ToString("yyyy/MM/dd");
        TextBox8.Text = DateTime.Now.AddDays(2).ToString("yyyy/MM/dd");
    }
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

    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='口味確認' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "PARANAME";
            DropDownList2.DataValueField = "PARANAME";
            DropDownList2.DataBind();

        }
        else
        {

        }


    }

    private void BindDropDownList3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='可行性' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList3.DataSource = dt;
            DropDownList3.DataTextField = "PARANAME";
            DropDownList3.DataValueField = "PARANAME";
            DropDownList3.DataBind();

        }
        else
        {

        }


    }

    private void BindDropDownList4()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='設計需求單' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList4.DataSource = dt;
            DropDownList4.DataTextField = "PARANAME";
            DropDownList4.DataValueField = "PARANAME";
            DropDownList4.DataBind();

        }
        else
        {

        }


    }

    private void BindDropDownList5()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='成本試算' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList5.DataSource = dt;
            DropDownList5.DataTextField = "PARANAME";
            DropDownList5.DataValueField = "PARANAME";
            DropDownList5.DataBind();

        }
        else
        {

        }


    }

    private void BindDropDownList6()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='校稿完成' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList6.DataSource = dt;
            DropDownList6.DataTextField = "PARANAME";
            DropDownList6.DataValueField = "PARANAME";
            DropDownList6.DataBind();

        }
        else
        {

        }


    }

    private void BindDropDownList7()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='試量產日期' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList7.DataSource = dt;
            DropDownList7.DataTextField = "PARANAME";
            DropDownList7.DataValueField = "PARANAME";
            DropDownList7.DataBind();

        }
        else
        {

        }


    }

    private void BindDropDownList8()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='正式量產日期' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList8.DataSource = dt;
            DropDownList8.DataTextField = "PARANAME";
            DropDownList8.DataValueField = "PARANAME";
            DropDownList8.DataBind();

        }
        else
        {

        }


    }

    private void BindDropDownList9()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='原料驗收作業' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList9.DataSource = dt;
            DropDownList9.DataTextField = "PARANAME";
            DropDownList9.DataValueField = "PARANAME";
            DropDownList9.DataBind();

        }
        else
        {

        }


    }

    private void BindDropDownList10()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='營養標示作業' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList10.DataSource = dt;
            DropDownList10.DataTextField = "PARANAME";
            DropDownList10.DataValueField = "PARANAME";
            DropDownList10.DataBind();

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
        string SERNO = "";      
        string STATUS = DropDownList1.SelectedValue.ToString().Trim();
        string SDATES = DateTime.Now.ToString("yyyy/MM/dd");
        string PRODUCTS = TextBox1.Text;
        string CLIENTS = "";
        string SALES = TextBox11.Text;
        string NUMS = "";
        string TESTDATES = "";
        string TESTMEMO = TextBox2.Text;
        string TASTESMEMO = DropDownList2.SelectedValue.ToString().Trim();
        string PACKAGES = TextBox4.Text;
        string FEASIBILITYS = DropDownList3.SelectedValue.ToString().Trim();
        string DESINGS = DropDownList4.SelectedValue.ToString().Trim();
        string DESINGSDATES = TextBox5.Text;
        string COSTS = DropDownList5.SelectedValue.ToString().Trim();
        string COSTSDATES = TextBox3.Text;
        string PROOFREADINGS = DropDownList6.SelectedValue.ToString().Trim();
        string PROOFREADINGSDATES = TextBox6.Text;
        string TESTPRODS = DropDownList7.SelectedValue.ToString().Trim();
        string PRODS = DropDownList8.SelectedValue.ToString().Trim();
        string ORICHECKS = DropDownList9.SelectedValue.ToString().Trim();
        string ORICHECKSDATES = TextBox7.Text;
        string NUTRICHECKS = DropDownList10.SelectedValue.ToString().Trim();
        string NUTRICHECKSDATES = TextBox8.Text;
        string REMARKS = TextBox12.Text;
        string CLOSEDDATES = TextBox13.Text;


        if (!string.IsNullOrEmpty(STATUS) && !string.IsNullOrEmpty(PRODUCTS))
        {
            ADDTBDEVNEW(
                      STATUS
                        , SDATES
                        , PRODUCTS
                        , CLIENTS
                        , SALES
                        , NUMS
                        , TESTDATES
                        , TESTMEMO
                        , TASTESMEMO
                        , PACKAGES
                        , FEASIBILITYS
                        , DESINGS
                        , DESINGSDATES
                        , COSTS
                        , COSTSDATES
                        , PROOFREADINGS
                        , PROOFREADINGSDATES
                        , TESTPRODS
                        , PRODS
                        , ORICHECKS
                        , ORICHECKSDATES
                        , NUTRICHECKS
                        , NUTRICHECKSDATES
                        , REMARKS
                        , CLOSEDDATES

                         );

        }




    }


    public void ADDTBDEVNEW(
                           string STATUS
                            , string SDATES
                            , string PRODUCTS
                            , string CLIENTS
                            , string SALES
                            , string NUMS
                            , string TESTDATES
                            , string TESTMEMO
                            , string TASTESMEMO
                            , string PACKAGES
                            , string FEASIBILITYS
                            , string DESINGS
                            , string DESINGSDATES
                            , string COSTS
                            , string COSTSDATES
                            , string PROOFREADINGS
                            , string PROOFREADINGSDATES
                            , string TESTPRODS
                            , string PRODS
                            , string ORICHECKS
                            , string ORICHECKSDATES
                            , string NUTRICHECKS
                            , string NUTRICHECKSDATES
                            , string REMARKS
                            , string CLOSEDDATES
                            )
    {
        //SDATES是空的或不是日期
        if (string.IsNullOrEmpty(SDATES)||!IsDate(SDATES))
        {
            SDATES = "1911/1/1";
        }


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

      
        string cmdTxt = @" 
                        INSERT INTO [TKRESEARCH].[dbo].[TBDEVNEW]
                        (                       
                         STATUS
                        , SDATES
                        , PRODUCTS
                        , CLIENTS
                        , SALES
                        , NUMS
                        , TESTDATES
                        , TESTMEMO
                        , TASTESMEMO
                        , PACKAGES
                        , FEASIBILITYS
                        , DESINGS
                        , DESINGSDATES
                        , COSTS
                        , COSTSDATES
                        , PROOFREADINGS
                        , PROOFREADINGSDATES
                        , TESTPRODS
                        , PRODS
                        , ORICHECKS
                        , ORICHECKSDATES
                        , NUTRICHECKS
                        , NUTRICHECKSDATES
                        , REMARKS
                        , CLOSEDDATES
                        )
                        VALUES
                        (
                         @STATUS
                        , @SDATES
                        , @PRODUCTS
                        , @CLIENTS
                        , @SALES
                        , @NUMS
                        , @TESTDATES
                        , @TESTMEMO
                        , @TASTESMEMO
                        , @PACKAGES
                        , @FEASIBILITYS
                        , @DESINGS
                        , @DESINGSDATES
                        , @COSTS
                        , @COSTSDATES
                        , @PROOFREADINGS
                        , @PROOFREADINGSDATES
                        , @TESTPRODS
                        , @PRODS
                        , @ORICHECKS
                        , @ORICHECKSDATES
                        , @NUTRICHECKS
                        , @NUTRICHECKSDATES
                        , @REMARKS
                        , @CLOSEDDATES
                        )

                            ";

     
        m_db.AddParameter("@STATUS", STATUS);
        m_db.AddParameter("@SDATES", SDATES);
        m_db.AddParameter("@PRODUCTS", PRODUCTS);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@SALES", SALES);
        m_db.AddParameter("@NUMS", NUMS);
        m_db.AddParameter("@TESTDATES", TESTDATES);
        m_db.AddParameter("@TESTMEMO", TESTMEMO);
        m_db.AddParameter("@TASTESMEMO", TASTESMEMO);
        m_db.AddParameter("@PACKAGES", PACKAGES);
        m_db.AddParameter("@FEASIBILITYS", FEASIBILITYS);
        m_db.AddParameter("@DESINGS", DESINGS);
        m_db.AddParameter("@DESINGSDATES", DESINGSDATES);
        m_db.AddParameter("@COSTS", COSTS);
        m_db.AddParameter("@COSTSDATES", COSTSDATES);
        m_db.AddParameter("@PROOFREADINGS", PROOFREADINGS);
        m_db.AddParameter("@PROOFREADINGSDATES", PROOFREADINGSDATES);
        m_db.AddParameter("@TESTPRODS", TESTPRODS);
        m_db.AddParameter("@PRODS", PRODS);
        m_db.AddParameter("@ORICHECKS", ORICHECKS);
        m_db.AddParameter("@ORICHECKSDATES", ORICHECKSDATES);
        m_db.AddParameter("@NUTRICHECKS", NUTRICHECKS);
        m_db.AddParameter("@NUTRICHECKSDATES", NUTRICHECKSDATES);
        m_db.AddParameter("@REMARKS", REMARKS);
        m_db.AddParameter("@CLOSEDDATES", CLOSEDDATES);




        m_db.ExecuteNonQuery(cmdTxt);

    }
    #endregion


}
