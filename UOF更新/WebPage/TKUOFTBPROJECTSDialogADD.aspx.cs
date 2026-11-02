using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CDS_WebPage_TKUOFTBPROJECTSDialogADD : Ede.Uof.Utility.Page.BasePage
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
        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();
    }

    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        ADD();
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

    public void ADD()
    {
        Guid ID = Guid.NewGuid();
        //string SERNO = "";
        //string NO = TextBox1.Text.Trim();
        string NO = GETMAXMO();
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


        if (!string.IsNullOrEmpty(NO) && !string.IsNullOrEmpty(SUBJECT) )
        {
            ADDTBPROJECTS(ID, NO, SUBJECT, CREATE_NEME, CREATE_DEP, CREATE_TIME, STATUS, CONTENTS, PUCLOSE_TIME,HADNUMS, HADCONTENTS, NOWNUMS, TOTALNUMS, CLOSE_CONTENTS, CLOSE_TIME);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDTBPROJECTS(Guid ID,string NO, string SUBJECT, string CREATE_NEME, string CREATE_DEP, string CREATE_TIME, string STATUS, string CONTENTS, string PUCLOSE_TIME, string HADNUMS, string HADCONTENTS, string NOWNUMS, string TOTALNUMS, string CLOSE_CONTENTS, string CLOSE_TIME)
    {
       

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKQC].[dbo].[TBPROJECTS]
                        ([ID],[NO],[SUBJECT],[CREATE_NEME],[CREATE_DEP],[CREATE_TIME],[STATUS],[CONTENTS],[PUCLOSE_TIME],[HADNUMS],[HADCONTENTS],[NOWNUMS],[TOTALNUMS],[CLOSE_CONTENTS],[CLOSE_TIME])
                        VALUES
                        (@ID,@NO,@SUBJECT,@CREATE_NEME,@CREATE_DEP,@CREATE_TIME,@STATUS,@CONTENTS,@PUCLOSE_TIME,@HADNUMS,@HADCONTENTS,@NOWNUMS,@TOTALNUMS,@CLOSE_CONTENTS,@CLOSE_TIME)

                            ";
        m_db.AddParameter("@ID", ID);
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

    public string GETMAXMO()
    {
        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlComm = new SqlCommand();
        string connectionString;
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();
        StringBuilder sbSql = new StringBuilder();

        string MAXNO;

        try
        {
            connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
            sqlConn = new SqlConnection(connectionString);

           
            sbSql.Clear();

            ds1.Clear();

            sbSql.AppendFormat(@"  
                                SELECT ISNULL(MAX([NO]),'0000000000') AS NO
                                FROM [TKQC].[dbo].[TBPROJECTS]
                                WHERE [NO] LIKE '%{0}%'
                                ",DateTime.Now.ToString("yyyyMM"));

            adapter1 = new SqlDataAdapter(@"" + sbSql, sqlConn);

            sqlCmdBuilder1 = new SqlCommandBuilder(adapter1);
            sqlConn.Open();
            ds1.Clear();
            adapter1.Fill(ds1, "ds1");
            sqlConn.Close();


            if (ds1.Tables["ds1"].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                if (ds1.Tables["ds1"].Rows.Count >= 1)
                {
                    MAXNO = SETMAXNO(ds1.Tables["ds1"].Rows[0]["NO"].ToString());
                    return MAXNO;

                }
                return null;
            }

        }
        catch
        {
            return null;
        }
        finally
        {
            sqlConn.Close();
        }
    }

    public string SETMAXNO(string MAXNO)
    {
        if (MAXNO.Equals("0000000000"))
        {
            return DateTime.Now.ToString("yyyyMM") + "0001";
        }

        else
        {
            int serno = Convert.ToInt16(MAXNO.Substring(6, 4));
            serno = serno + 1;
            string temp = serno.ToString();
            temp = temp.PadLeft(4, '0');
            return DateTime.Now.ToString("yyyyMM") + temp.ToString();
        }
    }

        #endregion


    }
