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


public partial class CDS_WebPage_TBBU_PRODUCTS_OTHERSDialogEDITDEL: Ede.Uof.Utility.Page.BasePage
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
                SEARCH_PRODUCTS_OTHERS(lblParam.Text);
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

        SEARCH_PRODUCTS_OTHERS(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DEL_PRODUCTS_OTHERS(lblParam.Text);
    }


    #endregion

    #region FUNCTION


    public void SEARCH_PRODUCTS_OTHERS(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                      SELECT *
                        FROM [TKBUSINESS].[dbo].[PRODUCTS_OTHERS]                        
                        WHERE [PRODUCTS_OTHERS].[MB001]=@MB001
                        ";
        m_db.AddParameter("@MB001", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["COMPANYS"].ToString();
            TextBox2.Text = dt.Rows[0]["MB001"].ToString();
            TextBox3.Text = dt.Rows[0]["MB002"].ToString();
            TextBox4.Text = dt.Rows[0]["MB003"].ToString();
            TextBox5.Text = dt.Rows[0]["MB004"].ToString();
            TextBox6.Text = dt.Rows[0]["MA003"].ToString();
            TextBox7.Text = dt.Rows[0]["MD007"].ToString();
            TextBox8.Text = dt.Rows[0]["MB093094095"].ToString();
            TextBox9.Text = dt.Rows[0]["VALIDITYPERIOD"].ToString();
            TextBox10.Text = dt.Rows[0]["MB013"].ToString();
            TextBox11.Text = dt.Rows[0]["PRODUCTSFEATURES"].ToString();
            TextBox12.Text = dt.Rows[0]["SALESFOCUS"].ToString();
            TextBox13.Text = dt.Rows[0]["PRICES1"].ToString();
            TextBox14.Text = dt.Rows[0]["PRICES2"].ToString();
            TextBox15.Text = dt.Rows[0]["PRICES3"].ToString();
            TextBox16.Text = dt.Rows[0]["MOQS"].ToString();

        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string COMPANYS = TextBox1.Text;
        string MB001 = TextBox2.Text;
        string MB002 = TextBox3.Text;
        string MB003 = TextBox4.Text;
        string MB004 = TextBox5.Text;
        string MA003 = TextBox6.Text;
        string MD007 = TextBox7.Text;
        string MB093094095 = TextBox8.Text;
        string VALIDITYPERIOD = TextBox9.Text;
        string MB013 = TextBox10.Text;
        string PRODUCTSFEATURES = TextBox11.Text;
        string SALESFOCUS = TextBox12.Text;
        string PRICES1 = TextBox13.Text;
        string PRICES2 = TextBox14.Text;
        string PRICES3 = TextBox15.Text;
        string MOQS = TextBox16.Text;

        if (!string.IsNullOrEmpty(ID) )
        {
            UPDATE_PRODUCTS_OTHERS(
                                 ID,
                                    COMPANYS ,
                                    MB001,
                                    MB002,
                                    MB003,
                                    MB004,
                                    MA003,
                                    MD007 ,
                                    MB093094095 ,
                                    VALIDITYPERIOD ,
                                    MB013,
                                    PRODUCTSFEATURES,
                                    SALESFOCUS ,
                                    PRICES1,
                                    PRICES2 ,
                                    PRICES3,
                                    MOQS 
                                        );
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATE_PRODUCTS_OTHERS(
                                    string ID,
                                    string COMPANYS,
                                    string MB001,
                                    string MB002,
                                    string MB003,
                                    string MB004,
                                    string MA003,
                                    string MD007,
                                    string MB093094095,
                                    string VALIDITYPERIOD,
                                    string MB013,
                                    string PRODUCTSFEATURES,
                                    string SALESFOCUS,
                                    string PRICES1,
                                    string PRICES2,
                                    string PRICES3,
                                    string MOQS
                                    )
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"                        
                            UPDATE  [TKBUSINESS].[dbo].[PRODUCTS_OTHERS]
                            SET 
                            [COMPANYS]=@COMPANYS
                            ,[PRODUCTSFEATURES]=@PRODUCTSFEATURES
                            ,[SALESFOCUS]=@SALESFOCUS
                            ,[COPYWRITINGS]=@COPYWRITINGS
                            ,[PICPATHS]=@PICPATHS
                            ,[PRICES1]=@PRICES1
                            ,[PRICES2]=@PRICES2
                            ,[PRICES3]=@PRICES3
                            ,[MOQS]=@MOQS
                            ,[MB002]=@MB002
                            ,[MB003]=@MB003
                            ,[MB004]=@MB004
                            ,[MA003]=@MA003
                            ,[MD007]=@MD007
                            ,[VALIDITYPERIOD]=@VALIDITYPERIOD
                            ,[MB047]=@MB047
                            ,[MB013]=@MB013
                            ,[MB093094095]=@MB093094095
                            WHERE [MB001]=@MB001
                   
                            ";


        m_db.AddParameter("@MB001", ID);
        m_db.AddParameter("@COMPANYS", COMPANYS);
        m_db.AddParameter("@PRODUCTSFEATURES", PRODUCTSFEATURES);
        m_db.AddParameter("@SALESFOCUS", SALESFOCUS);
        m_db.AddParameter("@COPYWRITINGS", "");
        m_db.AddParameter("@PICPATHS", "");
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
        m_db.AddParameter("@MB047", PRICES1);
        m_db.AddParameter("@MB013", MB013);
        m_db.AddParameter("@MB093094095", MB093094095);



        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DEL_PRODUCTS_OTHERS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                            DELETE [TKBUSINESS].[dbo].[PRODUCTS_OTHERS]
                            WHERE [MB001]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

    
    #endregion


}
