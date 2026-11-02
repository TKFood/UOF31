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


public partial class CDS_WebPage_TBBU_PRODUCTSDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //不顯示子視窗的按鈕
        //((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;
        ((Master_DialogMasterPage)this.Master).Button3Text = string.Empty;

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


            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHTPRODUCTS(lblParam.Text);
            }

        }

    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE();

        Dialog.SetReturnValue2("REFRESH");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE();

        SEARCHTPRODUCTS(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DEL_PRODUCTS(lblParam.Text);
    }


    #endregion

    #region FUNCTION


    public void SEARCHTPRODUCTS(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                      SELECT [PRODUCTS].[MB001],[PRODUCTSFEATURES],[SALESFOCUS],[COPYWRITINGS],[PICPATHS]
                        ,[PRICES1],[PRICES2],[PRICES3]
                        ,[MB002],[MOQS]
                        FROM [TKBUSINESS].[dbo].[PRODUCTS]
                        LEFT JOIN [TK].dbo.[INVMB] ON [PRODUCTS].[MB001]=[INVMB].[MB001]
                        WHERE [PRODUCTS].[MB001]=@MB001
                        ";
        m_db.AddParameter("@MB001", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["MB002"].ToString();
            TextBox2.Text = dt.Rows[0]["PRODUCTSFEATURES"].ToString();
            TextBox3.Text = dt.Rows[0]["SALESFOCUS"].ToString();
            TextBox4.Text = dt.Rows[0]["PRICES1"].ToString();
            TextBox5.Text = dt.Rows[0]["PRICES2"].ToString();
            TextBox6.Text = dt.Rows[0]["PRICES3"].ToString();
            TextBox7.Text = dt.Rows[0]["MOQS"].ToString();

        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string MB002 = TextBox1.Text;
        string PRODUCTSFEATURES = TextBox2.Text;
        string SALESFOCUS = TextBox3.Text;
        string COPYWRITINGS = "";
        string PRICES1 = TextBox4.Text;
        string PRICES2 = TextBox5.Text;
        string PRICES3 = TextBox6.Text;
        string MOQS= TextBox7.Text;


        if (!string.IsNullOrEmpty(ID) )
        {
            UPDATEPRODUCTS(ID, PRODUCTSFEATURES, SALESFOCUS, COPYWRITINGS, PRICES1, PRICES2, PRICES3, MOQS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATEPRODUCTS(string ID, string PRODUCTSFEATURES, string SALESFOCUS, string COPYWRITINGS, string PRICES1, string PRICES2, string PRICES3,string MOQS)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKBUSINESS].[dbo].[PRODUCTS]
                        SET 
                        [PRODUCTSFEATURES]=@PRODUCTSFEATURES
                        ,[SALESFOCUS]=@SALESFOCUS
                        ,[COPYWRITINGS]=@COPYWRITINGS
                        ,[PRICES1]=@PRICES1
                        ,[PRICES2]=@PRICES2
                        ,[PRICES3]=@PRICES3
                        ,[MOQS]=@MOQS
                        WHERE [MB001]=@ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@PRODUCTSFEATURES", PRODUCTSFEATURES);
        m_db.AddParameter("@SALESFOCUS", SALESFOCUS);
        m_db.AddParameter("@COPYWRITINGS", COPYWRITINGS);
        m_db.AddParameter("@PRICES1", PRICES1);
        m_db.AddParameter("@PRICES2", PRICES2);
        m_db.AddParameter("@PRICES3", PRICES3);
        m_db.AddParameter("@MOQS", MOQS);



        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DEL_PRODUCTS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE [TKBUSINESS].[dbo].[PRODUCTS] WHERE [MB001]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

    
    #endregion


}
