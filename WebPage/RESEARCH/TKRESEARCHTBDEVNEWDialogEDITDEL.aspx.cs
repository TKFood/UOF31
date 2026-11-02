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

        //不顯示子視窗的按鈕
        //((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;
        //((Master_DialogMasterPage)this.Master).Button3Text = string.Empty;


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
            BindDropDownList4();
            BindDropDownList5();
            BindDropDownList6();
            BindDropDownList7();
            BindDropDownList8();
            BindDropDownList9();
            BindDropDownList10();

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
                        ,[TASTESMEMO]
                        ,[PACKAGES]
                        ,[FEASIBILITYS]
                        ,[DESINGS]
                        ,CONVERT(NVARCHAR(10),[DESINGSDATES] ,111) AS DESINGSDATES
                        ,[COSTS]
                       ,CONVERT(NVARCHAR(10),[COSTSDATES] ,111) AS COSTSDATES 
                        ,[PROOFREADINGS]
                        ,CONVERT(NVARCHAR(10),[PROOFREADINGSDATES] ,111) AS PROOFREADINGSDATES
                        ,[TESTPRODS]
                        ,[PRODS]
                        ,[ORICHECKS]
                        , CONVERT(NVARCHAR(10),[ORICHECKSDATES],111) AS ORICHECKSDATES
                        ,[NUTRICHECKS]
                        , CONVERT(NVARCHAR(10),[NUTRICHECKSDATES],111) AS NUTRICHECKSDATES
                        ,[REMARKS]
                        ,[CLOSEDDATES]       

                        FROM [TKRESEARCH].[dbo].[TBDEVNEW]
                        WHERE [SERNO]=@SERNO 
                        ";


        m_db.AddParameter("@SERNO", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.Text = dt.Rows[0]["STATUS"].ToString();

            TextBox1.Text = dt.Rows[0]["PRODUCTS"].ToString();
            TextBox2.Text = dt.Rows[0]["TESTMEMO"].ToString();
            DropDownList2.Text = dt.Rows[0]["TASTESMEMO"].ToString();
            TextBox4.Text = dt.Rows[0]["PACKAGES"].ToString();
            DropDownList3.Text = dt.Rows[0]["FEASIBILITYS"].ToString();
            DropDownList4.Text = dt.Rows[0]["DESINGS"].ToString();
            TextBox5.Text = dt.Rows[0]["DESINGSDATES"].ToString();
            DropDownList5.Text = dt.Rows[0]["COSTS"].ToString();
            TextBox3.Text = dt.Rows[0]["COSTSDATES"].ToString();
            DropDownList6.Text = dt.Rows[0]["PROOFREADINGS"].ToString();
            TextBox6.Text = dt.Rows[0]["PROOFREADINGSDATES"].ToString();
            DropDownList7.Text = dt.Rows[0]["TESTPRODS"].ToString();
            DropDownList8.Text = dt.Rows[0]["PRODS"].ToString();
            DropDownList9.Text = dt.Rows[0]["ORICHECKS"].ToString();
            TextBox7.Text = dt.Rows[0]["ORICHECKSDATES"].ToString();
            DropDownList10.Text = dt.Rows[0]["NUTRICHECKS"].ToString();
            TextBox8.Text = dt.Rows[0]["NUTRICHECKSDATES"].ToString();
            TextBox11.Text = dt.Rows[0]["SALES"].ToString();
            TextBox12.Text = dt.Rows[0]["REMARKS"].ToString();
            TextBox13.Text = dt.Rows[0]["CLOSEDDATES"].ToString();



        }




    }

    public void UPDATE(string ID)
    {
        string SERNO = lblParam.Text;
        string STATUS = DropDownList1.SelectedValue.ToString().Trim();
        string SDATES = DateTime.Now.ToString("yyyy/MM/dd");
        string PRODUCTS = TextBox1.Text;
        string CLIENTS = "";
        string SALES = TextBox11.Text;
        string NUMS = "";
        string TESTDATES = "";
        string TESTMEMO = TextBox2.Text;
        string TASTESMEMO = DropDownList2.SelectedValue.ToString().Trim();
        string PACKAGES = "";
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
            UPDATETBDEVNEW(
                        SERNO
                        , STATUS
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

        Dialog.SetReturnValue2("REFRESH");
    }
    public void UPDATETBDEVNEW(
                            string SERNO
                            , string STATUS
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
                        SET 
                        STATUS=@STATUS
                        , SDATES=@SDATES
                        , PRODUCTS=@PRODUCTS
                        , CLIENTS=@CLIENTS
                        , SALES=@SALES
                        , NUMS=@NUMS
                        , TESTDATES=@TESTDATES
                        , TESTMEMO=@TESTMEMO
                        , TASTESMEMO=@TASTESMEMO
                        , PACKAGES=@PACKAGES
                        , FEASIBILITYS=@FEASIBILITYS
                        , DESINGS=@DESINGS
                        , DESINGSDATES=@DESINGSDATES
                        , COSTS=@COSTS
                        , COSTSDATES=@COSTSDATES
                        , PROOFREADINGS=@PROOFREADINGS
                        , PROOFREADINGSDATES=@PROOFREADINGSDATES
                        , TESTPRODS=@TESTPRODS
                        , PRODS=@PRODS
                        , ORICHECKS=@ORICHECKS
                        , ORICHECKSDATES=@ORICHECKSDATES
                        , NUTRICHECKS=@NUTRICHECKS
                        , NUTRICHECKSDATES=@NUTRICHECKSDATES
                        , REMARKS=@REMARKS
                        , CLOSEDDATES=@CLOSEDDATES
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
