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


public partial class CDS_WebPage_TKUOFTBPROJECTSMEMOADD : Ede.Uof.Utility.Page.BasePage
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
        if(!string.IsNullOrEmpty(lblParam.Text)&& !string.IsNullOrEmpty(TextBox3.Text) && !string.IsNullOrEmpty(TextBox1.Text) )
        {
            ADDTBPROJECTSMEMO(lblParam.Text, TextBox3.Text.Trim(), TextBox1.Text);
            
        }
        
        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        if (!string.IsNullOrEmpty(lblParam.Text) && !string.IsNullOrEmpty(TextBox3.Text) && !string.IsNullOrEmpty(TextBox1.Text))
        {
            ADDTBPROJECTSMEMO(lblParam.Text, TextBox3.Text.Trim(), TextBox1.Text);

        }

        BindGrid(lblParam.Text);

        Dialog.SetReturnValue2("NeedPostBack");
    }


    #endregion


    #region FUNCTION



    private void BindGrid(string NO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[SNO],[SUBJECT],CONVERT(NVARCHAR,[CREATEDATES],112) AS [CREATEDATES] ,[MEMOS] FROM [TKQC].[dbo].[TBPROJECTSMEMO] WHERE [SNO]=@NO ORDER BY [CREATEDATES] DESC  ";

        m_db.AddParameter("@NO", NO);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();




    }

    public void SEARCHTBSALESDEVMEMODEV(string NO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[NO],[SUBJECT],[CREATE_NEME],[CREATE_DEP],[CREATE_TIME],[STATUS],[CONTENTS],[PUCLOSE_TIME],[HADNUMS],[HADCONTENTS],[NOWNUMS],[TOTALNUMS],[CLOSE_CONTENTS],[CLOSE_TIME] FROM [TKQC].[dbo].[TBPROJECTS] WHERE [NO]=@NO";

        m_db.AddParameter("@NO", NO);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            TextBox2.Text = dt.Rows[0]["NO"].ToString();
            TextBox3.Text = dt.Rows[0]["SUBJECT"].ToString();
        }

        
    }

    public void ADDTBPROJECTSMEMO(string SNO,string SUBJECT,string MEMOS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  INSERT INTO [TKQC].[dbo].[TBPROJECTSMEMO]
                            ([ID],[SNO],[SUBJECT],[CREATEDATES],[MEMOS])
                            VALUES (@ID,@SNO,@SUBJECT,@CREATEDATES,@MEMOS)
                            ";

       
        m_db.AddParameter("@ID", Guid.NewGuid());
        m_db.AddParameter("@SNO", SNO);
        m_db.AddParameter("@SUBJECT", SUBJECT);
        m_db.AddParameter("@CREATEDATES", Convert.ToDateTime(DateTime.Now));  
        m_db.AddParameter("@MEMOS", MEMOS);

        m_db.ExecuteNonQuery(cmdTxt);

        
    }

   


    #endregion


}
