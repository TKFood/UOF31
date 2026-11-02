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


public partial class CDS_WebPage_TKRESEARCHTBSAMPLERECORDSDialogSALESADD : Ede.Uof.Utility.Page.BasePage
{
    string ID = null;
    string COMMENTS = null;
    string FIRSTADDDATES = null;
    string TBSALESEVENTSMAXID = null;
    string TBSALESEVENTSCOMMENTSMAXID = null;

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
            ID = Request["ID"];

            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                BindGrid(lblParam.Text);
                
            }

            //SEARCHTBSALESEVENTS(lblParam.Text);

            ////檢查[TBSALESEVENTSCOMMENTS]是否是空的，如果是就自動新增一筆
            //int ROWOCUNTS = SEARCHTBSALESEVENTSCOMMENTS(ID);
            //if(ROWOCUNTS==0)
            //{
            //    ADDTBSALESEVENTSCOMMENTSFIRST(lblParam.Text, TextBox1.Text, "", FIRSTADDDATES);
            //}



        }

    }




    #region BUTTON
  
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        if (!string.IsNullOrEmpty(lblParam.Text) && !string.IsNullOrEmpty(TextBox1.Text))
        {
            ADDTBSAMPLERECORDS(lblParam.Text, TextBox1.Text, DateTime.Now);
            UPDATETBSAMPLE(lblParam.Text, TextBox1.Text, DateTime.Now);

         
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        if (!string.IsNullOrEmpty(lblParam.Text) && !string.IsNullOrEmpty(TextBox1.Text))
        {
            ADDTBSAMPLERECORDS(lblParam.Text, TextBox1.Text, DateTime.Now);
            UPDATETBSAMPLE(lblParam.Text, TextBox1.Text, DateTime.Now);


        }

        BindGrid(lblParam.Text);

        Dialog.SetReturnValue2("NeedPostBack");
    }


    #endregion


    #region FUNCTION
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

   
    private void BindGrid(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                            SELECT 
                            [ID]
                            ,[MID]
                            ,CONVERT(NVARCHAR, [ADDDATES],120) AS ADDDATES 
                            ,REPLACE([COMMENTS],char(10),'<br/>') AS [COMMENTS] 

                            FROM [TKRESEARCH].[dbo].[TBSAMPLERECORDS]
                            WHERE [MID]=@ID     
                            ORDER BY [ADDDATES] DESC
                            ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();




    }

    public void SEARCHTBSALESEVENTS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[FORMID]
                        ,[DV01]
                        ,[DVV01]
                        ,[ISCLOSE]
                        FROM [TKRESEARCH].[dbo].[TBSAMPLE]
                        WHERE [ID]=@ID    
                        ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {          

            COMMENTS = dt.Rows[0]["COMMENTS"].ToString();
            FIRSTADDDATES = dt.Rows[0]["ADDDATES"].ToString();
        }


    }

    public void ADDTBSAMPLERECORDS(string MID, string COMMENTS, DateTime ADDDATES)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKRESEARCH].[dbo].[TBSAMPLERECORDS]
                        ([MID],[COMMENTS],[ADDDATES])
                        VALUES
                        (@MID,@COMMENTS,@ADDDATES)
                            ";

       
        
        m_db.AddParameter("@MID", MID);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@ADDDATES", Convert.ToDateTime(DateTime.Now));


        m_db.ExecuteNonQuery(cmdTxt);

      

    }

  
  
    public void UPDATETBSAMPLE(string ID, string COMMENTS, DateTime UPDATEDATES)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        UPDATE [TKRESEARCH].[dbo].[TBSAMPLE]
                        SET [COMMENTS]=@COMMENTS,[UPDATEDATES]=@UPDATEDATES
                        WHERE [ID]=@ID
                        ";

        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@UPDATEDATES", Convert.ToDateTime(DateTime.Now));

        m_db.ExecuteNonQuery(cmdTxt);

        
    }

 

   

  
    #endregion


}
