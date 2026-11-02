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


public partial class CDS_WebPage_TBBU_PRODUCTSDialogADDPRODUCTS_OTHERS : Ede.Uof.Utility.Page.BasePage
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
       
        string MB001 = TextBox2.Text;
        string COMPANYS = TextBox1.Text;
        string PRODUCTSFEATURES = TextBox11.Text;
        string SALESFOCUS = TextBox12.Text;
        string COPYWRITINGS = TextBox1.Text;
        string PICPATHS ="";
        string PRICES1 = TextBox13.Text;
        string PRICES2 = TextBox14.Text;
        string PRICES3 = TextBox15.Text;
        string MOQS = TextBox16.Text;
        string MB002 = TextBox3.Text;
        string MB003 = TextBox4.Text;
        string MB004 = TextBox5.Text;
        string MA003 = TextBox6.Text;
        string MD007 = TextBox7.Text;
        string VALIDITYPERIOD = TextBox9.Text;
        string MB047 = TextBox13.Text;
        string MB013 = TextBox10.Text;
        string MB093094095 = TextBox8.Text;

        if ( !string.IsNullOrEmpty(MB001))
        {

            ADD_PRODUCTS_OTHERS(
            MB001 ,
            COMPANYS ,
            PRODUCTSFEATURES ,
            SALESFOCUS ,
            COPYWRITINGS,
            PICPATHS ,
            PRICES1 ,
            PRICES2 ,
            PRICES3 ,
            MOQS ,
            MB002 ,
            MB003 ,
            MB004 ,
            MA003 ,
            MD007 ,
            VALIDITYPERIOD ,
            MB047,
            MB013,
            MB093094095 
            );
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADD_PRODUCTS_OTHERS(
                        string MB001 ,
                        string COMPANYS,
                        string PRODUCTSFEATURES,
                        string SALESFOCUS,
                        string COPYWRITINGS,
                        string PICPATHS,
                        string PRICES1,
                        string PRICES2,
                        string PRICES3,
                        string MOQS,
                        string MB002,
                        string MB003,
                        string MB004,
                        string MA003,
                        string MD007,
                        string VALIDITYPERIOD,
                        string MB047,
                        string MB013,
                        string MB093094095
                            )
    {
        Label8.Text = "";

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        try
        {
            string cmdTxt = @"                        
                            INSERT INTO  [TKBUSINESS].[dbo].[PRODUCTS_OTHERS]
                            (
                            [MB001]
                            ,[COMPANYS]
                            ,[PRODUCTSFEATURES]
                            ,[SALESFOCUS]
                            ,[COPYWRITINGS]
                            ,[PICPATHS]
                            ,[PRICES1]
                            ,[PRICES2]
                            ,[PRICES3]
                            ,[MOQS]
                            ,[MB002]
                            ,[MB003]
                            ,[MB004]
                            ,[MA003]
                            ,[MD007]
                            ,[VALIDITYPERIOD]
                            ,[MB047]
                            ,[MB013]
                            ,[MB093094095]
                            )
                            VALUES
                            (
                            @MB001
                            ,@COMPANYS
                            ,@PRODUCTSFEATURES
                            ,@SALESFOCUS
                            ,@COPYWRITINGS
                            ,@PICPATHS
                            ,@PRICES1
                            ,@PRICES2
                            ,@PRICES3
                            ,@MOQS
                            ,@MB002
                            ,@MB003
                            ,@MB004
                            ,@MA003
                            ,@MD007
                            ,@VALIDITYPERIOD
                            ,@MB047
                            ,@MB013
                            ,@MB093094095
                            )
                            ";



            m_db.AddParameter("@MB001", MB001);
            m_db.AddParameter("@COMPANYS", COMPANYS);
            m_db.AddParameter("@PRODUCTSFEATURES", PRODUCTSFEATURES);
            m_db.AddParameter("@SALESFOCUS", SALESFOCUS);
            m_db.AddParameter("@COPYWRITINGS", COPYWRITINGS);
            m_db.AddParameter("@PICPATHS", PICPATHS);
            m_db.AddParameter("@PRICES1", PRICES1);
            m_db.AddParameter("@PRICES2", PRICES2);
            m_db.AddParameter("@PRICES3", PRICES3);
            m_db.AddParameter("@MOQS", MOQS);
            m_db.AddParameter("@MB002", MB002);
            m_db.AddParameter("@MB003", MB003);
            m_db.AddParameter("@MB004", MB004);
            m_db.AddParameter("@MA003", MA003);
            m_db.AddParameter("@MD007", MD007);
            m_db.AddParameter("@VALIDITYPERIOD", VALIDITYPERIOD);
            m_db.AddParameter("@MB047", MB047);
            m_db.AddParameter("@MB013", MB013);
            m_db.AddParameter("@MB093094095", MB093094095);




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
