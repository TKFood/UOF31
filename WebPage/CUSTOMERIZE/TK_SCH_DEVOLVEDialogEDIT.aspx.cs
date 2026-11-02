using Ede.Uof.EIP.SystemInfo;
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


public partial class CDS_WebPage_CUSTOMERIZE_TK_SCH_DEVOLVEDialogEDIT : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    string ROLES = null;
    string USER_GUID = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        ROLES = SEARCHROLES(ACCOUNT.Trim());
        USER_GUID = SEARCHTB_EB_USER(ACCOUNT);
        NAMES.Text = NAME;

        //測試用
        //USER_GUID = "5af1f6e3-426e-4fc6-86ee-235845cba61e";

        //不顯示子視窗的按鈕
        //確定
        //((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        //確定後繼續
        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;
        //關閉
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
            //lblParam.Text = "f62e152b-c2f6-46af-9b96-267214391665";

            BindDropDownList();

            if (!string.IsNullOrEmpty(lblParam.Text)&& !string.IsNullOrEmpty(USER_GUID))
            {
                SEARCH_TB_EIP_SCH_WORK(lblParam.Text, USER_GUID);
            }

        }

    }

    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("CODE", typeof(String));
        dt.Columns.Add("NAMES", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"SELECT  
                                ID
                                ,KINDS
                                ,CODE
                                ,NAMES
                                FROM [UOF].[dbo].[Z_SET_PARA]
                                WHERE [KINDS]='交辨'
                                ORDER BY [ID]
                                ");

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "NAMES";
            DropDownList1.DataValueField = "NAMES";
            DropDownList1.DataBind();

        }
        else
        {

        }
    }


    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        string WORK_GUID = null;       
        string EXECUTE_USER = null;
        string WORK_STATE = "";
        string PROCEEDING_DESC = "";
        string COMPLETE_DESC = "";
        string DESCRIPTION = "";

        WORK_GUID = lblParam.Text;
        EXECUTE_USER = USER_GUID;

        if (DropDownList1.Text.ToString().Equals("進行中"))
        {
            WORK_STATE = "Proceeding";
            PROCEEDING_DESC = TextBox1.Text.ToString();
            COMPLETE_DESC = "";
            DESCRIPTION = TextBox1.Text.ToString();
        }
        else if (DropDownList1.Text.ToString().Equals("已完成"))
        {
            WORK_STATE = "Audit";
            PROCEEDING_DESC = "";
            COMPLETE_DESC = TextBox1.Text.ToString();
            DESCRIPTION = TextBox1.Text.ToString();
        }

        UPDATE_TB_EIP_SCH_WORK(WORK_GUID, EXECUTE_USER, WORK_STATE, PROCEEDING_DESC, COMPLETE_DESC, DESCRIPTION);


        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        Dialog.SetReturnValue2("OK");
       
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        string WORK_GUID = null; 
        string EXECUTE_USER = null; 
        string WORK_STATE = ""; 
        string PROCEEDING_DESC = ""; 
        string COMPLETE_DESC = "";
        string DESCRIPTION = "";

        WORK_GUID = lblParam.Text;
        EXECUTE_USER = USER_GUID;

        if (DropDownList1.Text.ToString().Equals("進行中"))
        {
            WORK_STATE = "Proceeding";
            PROCEEDING_DESC = TextBox1.Text.ToString();
            COMPLETE_DESC = "";
            DESCRIPTION = TextBox1.Text.ToString();
        }
        else if (DropDownList1.Text.ToString().Equals("已完成"))
        {
            WORK_STATE = "Audit";
            PROCEEDING_DESC = "";
            COMPLETE_DESC = TextBox1.Text.ToString();
            DESCRIPTION = TextBox1.Text.ToString();
        }

        UPDATE_TB_EIP_SCH_WORK(WORK_GUID, EXECUTE_USER, WORK_STATE, PROCEEDING_DESC, COMPLETE_DESC, DESCRIPTION);
        //設定回傳值並關閉視窗
        Dialog.SetReturnValue2("OK");

        Dialog.Close(this);
        //SEARCHCOPTD(lblParam.Text);
    }


    #endregion

    #region FUNCTION


    public string SEARCHROLES(string ACCOUNT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            SELECT  
                            [ID]
                            ,[ROLES]
                            ,[MV001]
                            ,[MV002]
                            FROM [TKBUSINESS].[dbo].[TBCOPTDCHECKROLES]
                            WHERE MV001 ='{0}'

                              ", ACCOUNT);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["ROLES"].ToString().Trim();
        }
        else
        {
            return "NOROLES";
        }

    }

    public string SEARCHTB_EB_USER(string ACCOUNT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            SELECT *
                            FROM [UOF].dbo.TB_EB_USER
                            WHERE ACCOUNT=@ACCOUNT

                              ", ACCOUNT);




        m_db.AddParameter("@ACCOUNT", ACCOUNT);
     
        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["USER_GUID"].ToString().Trim();
        }
        else
        {
            return null;
        }

    }
    public void SEARCH_TB_EIP_SCH_WORK(string WORK_GUID, string EXECUTE_USER)
    {
       
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                        SELECT *
                        ,(ISNULL(TB_EIP_SCH_WORK.PROCEEDING_DESC,'')+ISNULL(TB_EIP_SCH_WORK.COMPLETE_DESC,''))  AS '交辨回覆'
                        FROM [UOF].dbo.TB_EIP_SCH_WORK
                        WHERE WORK_GUID=@WORK_GUID
                        AND EXECUTE_USER=@EXECUTE_USER
                        ORDER BY CREATE_TIME DESC
                        ";
        m_db.AddParameter("@WORK_GUID", WORK_GUID);
        m_db.AddParameter("@EXECUTE_USER", EXECUTE_USER);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {

            TextBox1.ReadOnly = false;
            TextBox1.Text = dt.Rows[0]["交辨回覆"].ToString();
            TextBox2.Text = dt.Rows[0]["SUBJECT"].ToString();

        }
        else
        {
            TextBox1.ReadOnly = true;
            TextBox1.Text = "交辨事項及交辨人不正確，無法填寫!";
            TextBox2.Text = "交辨事項及交辨人不正確，無法填寫!";

            ((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        }
    }





    public void UPDATE_TB_EIP_SCH_WORK(string WORK_GUID, string EXECUTE_USER, string WORK_STATE, string PROCEEDING_DESC, string COMPLETE_DESC, string DESCRIPTION)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            string cmdTxt = @"
            UPDATE [UOF].dbo.TB_EIP_SCH_WORK
            SET 
                [WORK_STATE] = @WORK_STATE,
                PROCEEDING_DESC = @PROCEEDING_DESC,
                [COMPLETE_DESC] = @COMPLETE_DESC,
                [COMPLETE_TIME] = @COMPLETE_TIME
            WHERE 
                [WORK_GUID] = @WORK_GUID 
                AND [EXECUTE_USER] = @EXECUTE_USER;

            INSERT INTO [UOF].[dbo].[TB_EIP_SCH_WORK_RECORD]
            (
                [WORK_RECORD_GUID],
                [WORK_GUID],
                [WORK_STATE],
                [DESCRIPTION],
                [CREATE_TIME],
                [CREATE_USER],
                [CREATE_FROM],
                [FILE_GROUP_GUID]
            )
            VALUES
            (
                NEWID(),
                @WORK_GUID,
                @WORK_STATE,
                @DESCRIPTION,
                GETDATE(),
                @CREATE_USER,
                '192.168.1.233',
                NULL
            );
        ";

            m_db.AddParameter("@WORK_GUID", WORK_GUID);
            m_db.AddParameter("@WORK_STATE", WORK_STATE);
            m_db.AddParameter("@PROCEEDING_DESC", PROCEEDING_DESC);
            m_db.AddParameter("@COMPLETE_DESC", COMPLETE_DESC);
            m_db.AddParameter("@COMPLETE_TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
            m_db.AddParameter("@EXECUTE_USER", EXECUTE_USER);
            m_db.AddParameter("@DESCRIPTION", DESCRIPTION);
            m_db.AddParameter("@CREATE_USER", EXECUTE_USER);  // 加上這個，否則 @CREATE_USER 無值會報錯

            m_db.ExecuteNonQuery(cmdTxt);
        }
        catch (Exception ex)
        {
            // 這邊可以加 Log 紀錄、或是顯示錯誤訊息
            Console.WriteLine("更新 TB_EIP_SCH_WORK 發生錯誤: " + ex.Message);

            // 你可以選擇拋出錯誤或記錄後繼續
            throw;
        }
    }



    #endregion




}
