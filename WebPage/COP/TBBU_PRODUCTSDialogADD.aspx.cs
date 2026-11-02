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


public partial class CDS_WebPage_TBBU_PRODUCTSDialogADD : Ede.Uof.Utility.Page.BasePage
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
            ////接收主頁面傳遞之參數
            //lblParam.Text = Request["ID"];


            //if (!string.IsNullOrEmpty(lblParam.Text))
            //{
            //   // SEARCHTCOPCONDTIONS(lblParam.Text);
            //}

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();

        Dialog.SetReturnValue2("REFRESH");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();
       
    }

    #endregion

    #region FUNCTION


    public void ADD()
    {
        Guid ID = Guid.NewGuid();
        //string SERNO = "";
        string MB001 = TextBox1.Text;
        string PRODUCTSFEATURES = TextBox2.Text;
        string SALESFOCUS = TextBox3.Text;
        string COPYWRITINGS = "";
        string PRICES1 = TextBox4.Text;
        string PRICES2 = TextBox5.Text;
        string PRICES3 = TextBox6.Text;
        string MOQS = TextBox7.Text;


        if ( !string.IsNullOrEmpty(MB001))
        {

            ADDPRODUCTS(MB001, PRODUCTSFEATURES, SALESFOCUS, COPYWRITINGS, PRICES1, PRICES2, PRICES3, MOQS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDPRODUCTS(string MB001, string PRODUCTSFEATURES, string SALESFOCUS, string COPYWRITINGS, string PRICES1, string PRICES2, string PRICES3,string MOQS)
    {
        Label8.Text = "";

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        try
        {
            string cmdTxt = @"  
                        INSERT INTO [TKBUSINESS].[dbo].[PRODUCTS]
                        ([MB001],[PRODUCTSFEATURES],[SALESFOCUS],[COPYWRITINGS],[PRICES1],[PRICES2],[PRICES3],[MOQS],[COMPANYS])
                        VALUES
                        (@MB001,@PRODUCTSFEATURES,@SALESFOCUS,@COPYWRITINGS,@PRICES1,@PRICES2,@PRICES3,@MOQS,@COMPANYS)         

                            ";



            m_db.AddParameter("@MB001", MB001);
            m_db.AddParameter("@PRODUCTSFEATURES", PRODUCTSFEATURES);
            m_db.AddParameter("@SALESFOCUS", SALESFOCUS);
            m_db.AddParameter("@COPYWRITINGS", COPYWRITINGS);
            m_db.AddParameter("@PRICES1", PRICES1);
            m_db.AddParameter("@PRICES2", PRICES2);
            m_db.AddParameter("@PRICES3", PRICES3);
            m_db.AddParameter("@MOQS", MOQS);
            m_db.AddParameter("@COMPANYS", "老楊");


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
