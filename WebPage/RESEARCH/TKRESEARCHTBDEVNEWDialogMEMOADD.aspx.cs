using Ede.Uof.Utility.Component;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CDS_WebPage_TKRESEARCHTBDEVNEWDialogMEMOADD : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //設定回傳值
        Dialog.SetReturnValue2("");

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

            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                BindGrid(lblParam.Text);
                SEARCHTBDEVNEW(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);
        if(!string.IsNullOrEmpty(lblParam.Text)&& !string.IsNullOrEmpty(TextBox2.Text) && !string.IsNullOrEmpty(TextBox1.Text) )
        {
            ADDTBDEVNEWMEMO(lblParam.Text,  TextBox1.Text);
            UPDATETBDEVNEW(lblParam.Text, TextBox1.Text);
        }
        
        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        if (!string.IsNullOrEmpty(lblParam.Text) && !string.IsNullOrEmpty(TextBox2.Text) && !string.IsNullOrEmpty(TextBox1.Text))
        {
            ADDTBDEVNEWMEMO(lblParam.Text,  TextBox1.Text);
            UPDATETBDEVNEW(lblParam.Text, TextBox1.Text);
        }

        BindGrid(lblParam.Text);

        Dialog.SetReturnValue2("NeedPostBack");
    }


    #endregion


    #region FUNCTION



    private void BindGrid(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT [SERNO]
                        ,[MSERNO]
                        ,CONVERT(NVARCHAR(10),[TESTDATES],111) AS 'TESTDATES'
                        ,[TESTMEMO]
                        FROM [TKRESEARCH].[dbo].[TBDEVNEWMEMO]
                        WHERE [MSERNO]=@ID
                        ORDER BY [TESTDATES] DESC   
                        ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();




    }

    public void SEARCHTBDEVNEW(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                       SELECT [SERNO]
                        ,[STATUS]
                        ,[SDATES]
                        ,[PRODUCTS]
                        ,[CLIENTS]
                        ,[SALES]
                        ,[NUMS]
                        ,[TESTDATES]
                        ,[TESTMEMO]
                        FROM [TKRESEARCH].[dbo].[TBDEVNEW]
                        WHERE [SERNO]=@ID
                        ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            TextBox2.Text = dt.Rows[0]["PRODUCTS"].ToString();
        }

        
    }

    public void ADDTBDEVNEWMEMO(string MSERNO, string TESTMEMO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKRESEARCH].[dbo].[TBDEVNEWMEMO]
                        ([MSERNO],[TESTDATES],[TESTMEMO])
                        VALUES
                        (@MSERNO,@TESTDATES,@TESTMEMO)
                        ";

       
        m_db.AddParameter("@MSERNO", MSERNO);
        m_db.AddParameter("@TESTDATES", Convert.ToDateTime(DateTime.Now));
        m_db.AddParameter("@TESTMEMO", TESTMEMO);

        m_db.ExecuteNonQuery(cmdTxt);

        
    }

    public void UPDATETBDEVNEW(string SERNO, string TESTMEMO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  UPDATE [TKRESEARCH].[dbo].[TBDEVNEW]
                            SET [TESTMEMO]=@TESTMEMO,[TESTDATES]=@TESTDATES
                            WHERE [SERNO]=@SERNO
                            ";

        m_db.AddParameter("@SERNO", SERNO);
        m_db.AddParameter("@TESTMEMO", TESTMEMO);
        m_db.AddParameter("@TESTDATES", Convert.ToDateTime(DateTime.Now));

        m_db.ExecuteNonQuery(cmdTxt);

        
    }


    #endregion


}
