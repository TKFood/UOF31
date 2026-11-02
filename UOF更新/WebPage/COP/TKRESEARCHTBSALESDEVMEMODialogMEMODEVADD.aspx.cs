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


public partial class CDS_WebPage_TKRESEARCHTBSALESDEVMEMODialogMEMODEVADD : Ede.Uof.Utility.Page.BasePage
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

            if (!string.IsNullOrEmpty(lblParam.Text))
            {                
                BindGrid(lblParam.Text);
                SEARCHTBSALESDEVMEMODEV(lblParam.Text);
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
            ADDTBSALESDEVMEMOHISTORYDEV(lblParam.Text, TextBox2.Text.Trim(), TextBox1.Text);
            UPDATETBSALESDEVMEMO(lblParam.Text, TextBox1.Text);
        }
        
        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        if (!string.IsNullOrEmpty(lblParam.Text) && !string.IsNullOrEmpty(TextBox2.Text) && !string.IsNullOrEmpty(TextBox1.Text))
        {
            ADDTBSALESDEVMEMOHISTORYDEV(lblParam.Text, TextBox2.Text.Trim(), TextBox1.Text);
            UPDATETBSALESDEVMEMO(lblParam.Text, TextBox1.Text);
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

        string cmdTxt = @" SELECT CONVERT(NVARCHAR,[MEMODATES],120) AS MEMODATES ,[PROD],[DEVMEMO],[ID],[PID] FROM [TKRESEARCH].[dbo].[TBSALESDEVMEMOHISTORYDEV] WHERE [PID]=@ID ORDER BY [MEMODATES] DESC   ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();




    }

    public void SEARCHTBSALESDEVMEMODEV(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[SERNO],[STATUS],[CLIENT],[PROD],[PRICES],[PROMOTIONS],[SPEC],[VALID],[PLACES],[ONSALES],[PRODESGIN],CONVERT(NVARCHAR,[ASSESSMENTDATES],111) ASSESSMENTDATES,CONVERT(NVARCHAR,[COSTSDATES],111) COSTSDATES,[SALESPRICES],[TEST],CONVERT(NVARCHAR,[TESTDATES],111) TESTDATES,[OWNER],[MEMO] FROM [TKRESEARCH].[dbo].[TBSALESDEVMEMO] WHERE [ID]=@ID  ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            TextBox2.Text = dt.Rows[0]["PROD"].ToString();
        }

        
    }

    public void ADDTBSALESDEVMEMOHISTORYDEV(string PID,string PROD, string DEVMEMO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  INSERT INTO [TKRESEARCH].[dbo].[TBSALESDEVMEMOHISTORYDEV]
                            ([ID],[PID],[MEMODATES],[PROD],[DEVMEMO])
                            VALUES (@ID,@PID,@MEMODATES,@PROD,@DEVMEMO)
                            ";

       
        m_db.AddParameter("@ID", Guid.NewGuid());
        m_db.AddParameter("@PID", PID);
        m_db.AddParameter("@MEMODATES", Convert.ToDateTime(DateTime.Now));
        m_db.AddParameter("@PROD", PROD);
        m_db.AddParameter("@DEVMEMO", DEVMEMO);

        m_db.ExecuteNonQuery(cmdTxt);

        
    }

    public void UPDATETBSALESDEVMEMO(string PID, string DEVMEMO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  UPDATE [TKRESEARCH].[dbo].[TBSALESDEVMEMO]
                            SET [DEVMEMO]=@DEVMEMO,[DEVMEMODATES]=@DEVMEMODATES
                            WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", PID);
        m_db.AddParameter("@DEVMEMO", DEVMEMO);
        m_db.AddParameter("@DEVMEMODATES", Convert.ToDateTime(DateTime.Now));

        m_db.ExecuteNonQuery(cmdTxt);

        
    }


    #endregion


}
