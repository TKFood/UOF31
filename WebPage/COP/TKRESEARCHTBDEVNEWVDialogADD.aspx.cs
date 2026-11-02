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
        string SERNO = "";
        string STATUS = DropDownList1.SelectedValue.ToString().Trim();
        string SDATES = DateTime.Now.ToString("yyyy/MM/dd");
        string PRODUCTS = TextBox1.Text;
        string CLIENTS = "";
        string SALES= TextBox11.Text;
        string NUMS= "";
        string TESTDATES= "";
        string TESTMEMO = TextBox2.Text;
        string TASTESMEMO = TextBox3.Text;
        string PACKAGES = TextBox4.Text;
        string FEASIBILITYS = TextBox5.Text;
        string DESINGS = TextBox6.Text;
        string COSTS = TextBox7.Text;
        string PROOFREADINGS = TextBox8.Text;
        string TESTPRODS = TextBox9.Text;
        string PRODS = TextBox10.Text;
        string REMARKS = TextBox12.Text;
        string CLOSEDDATES= TextBox13.Text;


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
                         , COSTS
                         , PROOFREADINGS
                         , TESTPRODS
                         , PRODS
                         , REMARKS
                         , CLOSEDDATES
                                 
                         );

        }
          
                

      
    }


    public void ADDTBDEVNEW( string STATUS
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
                            , string COSTS
                            , string PROOFREADINGS
                            , string TESTPRODS
                            , string PRODS
                            , string REMARKS
                            , string CLOSEDDATES
                                    )
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
                        (
                        STATUS
                        ,SDATES
                        ,PRODUCTS
                        ,CLIENTS
                        ,SALES
                        ,NUMS
                        ,TESTDATES
                        ,TESTMEMO
                        ,TASTESMEMO
                        ,PACKAGES
                        ,FEASIBILITYS
                        ,DESINGS
                        ,COSTS
                        ,PROOFREADINGS
                        ,TESTPRODS
                        ,PRODS
                        ,REMARKS
                        ,CLOSEDDATES
                        )
                        VALUES
                        (
                        @STATUS
                        ,@SDATES
                        ,@PRODUCTS
                        ,@CLIENTS
                        ,@SALES
                        ,@NUMS
                        ,@TESTDATES
                        ,@TESTMEMO
                        ,@TASTESMEMO
                        ,@PACKAGES
                        ,@FEASIBILITYS
                        ,@DESINGS
                        ,@COSTS
                        ,@PROOFREADINGS
                        ,@TESTPRODS
                        ,@PRODS
                        ,@REMARKS
                        ,@CLOSEDDATES
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
        m_db.AddParameter("@COSTS", COSTS);
        m_db.AddParameter("@PROOFREADINGS", PROOFREADINGS);
        m_db.AddParameter("@TESTPRODS", TESTPRODS);
        m_db.AddParameter("@PRODS", PRODS);
        m_db.AddParameter("@REMARKS", REMARKS);
        m_db.AddParameter("@CLOSEDDATES", CLOSEDDATES);


        m_db.ExecuteNonQuery(cmdTxt);

    }
    #endregion


}
