using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CDS_WebPage_TKUOFTBPROJECTSDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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

            BindDropDownList();

            BindGrid(lblParam.Text);

            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHTBPROJECTS(lblParam.Text);

                BindGrid(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE();

        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE();

        SEARCHTBPROJECTS(lblParam.Text);
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

        string cmdTxt = @" SELECT [ID],[KIND],[PARAID],[PARANAME] FROM [TKUOF].[dbo].[TBPARA] WHERE [KIND]='PROJECTSTATUS' ORDER BY [PARAID] ";

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

    public void SEARCHTBPROJECTS(string NO)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT  [ID],[NO],[SUBJECT],[CREATE_NEME],[CREATE_DEP],CONVERT(NVARCHAR,[CREATE_TIME],111) [CREATE_TIME],[STATUS],[CONTENTS],CONVERT(NVARCHAR,[PUCLOSE_TIME],111) [PUCLOSE_TIME],[HADNUMS],[HADCONTENTS],[NOWNUMS],[TOTALNUMS],[CLOSE_CONTENTS],CONVERT(NVARCHAR,[CLOSE_TIME],111) [CLOSE_TIME]
                        FROM [TKQC].[dbo].[TBPROJECTS]
                        WHERE [NO]=@NO
                        ";
        m_db.AddParameter("@NO", NO);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.Text = dt.Rows[0]["STATUS"].ToString();
            TextBox1.Text = dt.Rows[0]["NO"].ToString();
            TextBox2.Text = dt.Rows[0]["SUBJECT"].ToString();
            TextBox3.Text = dt.Rows[0]["CREATE_NEME"].ToString();
            TextBox4.Text = dt.Rows[0]["CREATE_DEP"].ToString();
            txtDate1.Text = dt.Rows[0]["CREATE_TIME"].ToString();
            TextBox5.Text = dt.Rows[0]["CONTENTS"].ToString();
            txtDate2.Text = dt.Rows[0]["PUCLOSE_TIME"].ToString();
            TextBox6.Text = dt.Rows[0]["HADNUMS"].ToString();
            TextBox7.Text = dt.Rows[0]["HADCONTENTS"].ToString();
            TextBox8.Text = dt.Rows[0]["NOWNUMS"].ToString();
            TextBox9.Text = dt.Rows[0]["TOTALNUMS"].ToString();
            TextBox10.Text = dt.Rows[0]["CLOSE_CONTENTS"].ToString();
            txtDate3.Text = dt.Rows[0]["CLOSE_TIME"].ToString();
    

        }

      


    }

    public void UPDATE()
    {
        string NO = TextBox1.Text.Trim();
        string SUBJECT = TextBox2.Text.Trim();
        string CREATE_NEME = TextBox3.Text.Trim();
        string CREATE_DEP = TextBox4.Text.Trim();
        string CREATE_TIME = txtDate1.Text.Trim();
        string STATUS = DropDownList1.Text.Trim();
        string CONTENTS = TextBox5.Text.Trim();
        string PUCLOSE_TIME = txtDate2.Text.Trim();
        string HADNUMS = TextBox6.Text.Trim();
        string HADCONTENTS = TextBox7.Text.Trim();
        string NOWNUMS = TextBox8.Text.Trim();
        string TOTALNUMS = TextBox9.Text.Trim();
        string CLOSE_CONTENTS = TextBox10.Text.Trim();
        string CLOSE_TIME = txtDate3.Text.Trim();


        if (!string.IsNullOrEmpty(NO) && !string.IsNullOrEmpty(SUBJECT))
        {
            UPDATETBPROJECTS(ID, NO, SUBJECT, CREATE_NEME, CREATE_DEP, CREATE_TIME, STATUS, CONTENTS, PUCLOSE_TIME, HADNUMS, HADCONTENTS, NOWNUMS, TOTALNUMS, CLOSE_CONTENTS, CLOSE_TIME);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void UPDATETBPROJECTS(string ID, string NO, string SUBJECT, string CREATE_NEME, string CREATE_DEP, string CREATE_TIME, string STATUS, string CONTENTS, string PUCLOSE_TIME, string HADNUMS, string HADCONTENTS, string NOWNUMS, string TOTALNUMS, string CLOSE_CONTENTS, string CLOSE_TIME)
    {
        if (string.IsNullOrEmpty(CREATE_TIME))
        {
            CREATE_TIME = "1911/1/1";   
        }
        if (string.IsNullOrEmpty(PUCLOSE_TIME))
        {
            PUCLOSE_TIME = "1911/1/1";
        }
        if (string.IsNullOrEmpty(CLOSE_TIME))
        {
            CLOSE_TIME = "1911/1/1";
        }


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKQC].[dbo].[TBPROJECTS]
                        SET [SUBJECT]=@SUBJECT,[CREATE_NEME]=@CREATE_NEME,[CREATE_DEP]=@CREATE_DEP,[CREATE_TIME]=@CREATE_TIME,[STATUS]=@STATUS,[CONTENTS]=@CONTENTS,[PUCLOSE_TIME]=@PUCLOSE_TIME,[HADNUMS]=@HADNUMS,[HADCONTENTS]=@HADCONTENTS,[NOWNUMS]=@NOWNUMS,[TOTALNUMS]=@TOTALNUMS,[CLOSE_CONTENTS]=@CLOSE_CONTENTS,[CLOSE_TIME]=@CLOSE_TIME
                        WHERE [NO]=@NO
                            ";


 
        m_db.AddParameter("@NO", NO);
        m_db.AddParameter("@SUBJECT", SUBJECT);
        m_db.AddParameter("@CREATE_NEME", CREATE_NEME);
        m_db.AddParameter("@CREATE_DEP", CREATE_DEP);
        m_db.AddParameter("@CREATE_TIME", CREATE_TIME);
        m_db.AddParameter("@STATUS", STATUS);
        m_db.AddParameter("@CONTENTS", CONTENTS);
        m_db.AddParameter("@PUCLOSE_TIME", PUCLOSE_TIME);
        m_db.AddParameter("@HADNUMS", HADNUMS);
        m_db.AddParameter("@HADCONTENTS", HADCONTENTS);
        m_db.AddParameter("@NOWNUMS", NOWNUMS);
        m_db.AddParameter("@TOTALNUMS", TOTALNUMS);
        m_db.AddParameter("@CLOSE_CONTENTS", CLOSE_CONTENTS);
        m_db.AddParameter("@CLOSE_TIME", CLOSE_TIME);

        m_db.ExecuteNonQuery(cmdTxt);



    }

    private void BindGrid(string NO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT [ID],[TaskId],[QCFrm002SN],[QCFrm002QCC],[QCFrm002PN]
                        FROM [TKQC].[dbo].[TBFORMQC]
                        WHERE [QCFrm002PN]=@NO
                        ORDER BY [QCFrm002SN],[QCFrm002QCC]
                        ";

        m_db.AddParameter("@NO", NO);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

            //ExpandoObject param = new { ID = row["QCFrm002SN"].ToString() }.ToExpando();
            ////Grid開窗是用RowDataBound事件再開窗

            //Dialog.Open2(lbtnName, "~/WKF/FormUse/PersonalBox/MyFormList.aspx?item=SignSelf&formNumber=BPM200400033&taskId=92db6bed-c946-42ed-ab4d-a415e5577b22", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
            ////Dialog.Open2(lbtnName, "~/WKF/FormUse/ViewForm.aspx?TASK_ID=92db6bed-c946-42ed-ab4d-a415e5577b22", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            string Taskid = FindTaskid(row["QCFrm002SN"].ToString());
            //string Taskid = "bf878ff0-357f-4817-9f2b-9c7671bfe076";

            //有判斷權限
            //ExpandoObject param = new { TASKID = Taskid }.ToExpando();

            //Dialog.Open2(lbtnName, "~/WKF/FormUse/Sign_View.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);


            //無判斷權限(直接觀看表單)
            ExpandoObject param = new { TASK_ID = Taskid }.ToExpando();

            Dialog.Open2(lbtnName, "~/WKF/FormUse/ViewForm.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);


        }

    }

    public string FindTaskid(string QCFrm002SN)
    {
        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlComm = new SqlCommand();
        string connectionString;
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();
        StringBuilder sbSql = new StringBuilder();

        try
        {
            connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
            sqlConn = new SqlConnection(connectionString);

            sbSql.Clear();

            sbSql.AppendFormat(@"  SELECT TOP 1 [TaskId] FROM [TKQC].[dbo].[TBFORMQC] WHERE [QCFrm002SN]='{0}'
                                   
                                    ", QCFrm002SN);

            adapter1 = new SqlDataAdapter(@"" + sbSql, sqlConn);

            sqlCmdBuilder1 = new SqlCommandBuilder(adapter1);
            sqlConn.Open();
            ds1.Clear();
            adapter1.Fill(ds1, "TEMPds1");
            sqlConn.Close();


            if (ds1.Tables["TEMPds1"].Rows.Count >= 1)
            {
                return ds1.Tables["TEMPds1"].Rows[0]["TaskId"].ToString();
            }
            else
            {
                return "";
            }

        }
        catch
        {
            return "";
        }
        finally
        {
            sqlConn.Close();
        }
    }

    public void OnBeforeExport(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        
    }

    #endregion

    #region BUTTON
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBPROJECTS(lblParam.Text);
    }

    public void DELTBPROJECTS(string NO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE [TKQC].[dbo].[TBPROJECTS]  WHERE[NO]=@NO
                            ";

        m_db.AddParameter("@NO", NO);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }


    #endregion


}
