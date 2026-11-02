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


public partial class CDS_WebPage_TBBU_TBPROJECTSDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
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
            BindDropDownList();
            //接收主頁面傳遞之參數
            lblParam.Text = Request["ID"];


            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHTTBPROJECTS(lblParam.Text);
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

        SEARCHTTBPROJECTS(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBPROJECTS(lblParam.Text);
    }


    protected void btn2_Click(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(TextBox7.Text))
        {
            ADDTBPROJECTS(TextBox7.Text);
        }
    }

    #endregion

    #region FUNCTION
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SALESFOCUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[STORES] FROM [TKBUSINESS].[dbo].[STORESKIND] ORDER BY ID";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "STORES";
            DropDownList1.DataValueField = "STORES";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }

    public void SEARCHTTBPROJECTS(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                        SELECT
                        [ID]
                        ,[YEARS]
                        ,[WEEKS]
                        ,[STORES]
                        ,[NAMES]
                        ,[ITEMS]
                        ,[CONTENTS]
                        ,[DAYS] 
                        FROM [TKBUSINESS].[dbo].[TBPROJECTS]
                        WHERE [ID]=@ID
                        ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {           
            TextBox1.Text = dt.Rows[0]["YEARS"].ToString();
            TextBox2.Text = dt.Rows[0]["WEEKS"].ToString();
            
            DropDownList1.SelectedValue = dt.Rows[0]["STORES"].ToString().Trim();
            TextBox3.Text = dt.Rows[0]["NAMES"].ToString();
            TextBox4.Text = dt.Rows[0]["ITEMS"].ToString();
            TextBox5.Text = dt.Rows[0]["CONTENTS"].ToString();
            TextBox6.Text = dt.Rows[0]["DAYS"].ToString();


        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string YEARS = TextBox1.Text;
        string WEEKS = TextBox2.Text;
        
        string STORES = DropDownList1.SelectedValue.Trim();
        string NAMES = TextBox3.Text;
        string ITEMS = TextBox4.Text;
        string CONTENTS = TextBox5.Text;
        string DAYS = TextBox6.Text;




        if (!string.IsNullOrEmpty(ID) )
        {
            UPDATETBPROJECTS(ID, YEARS, WEEKS, STORES, NAMES, ITEMS, CONTENTS, DAYS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBPROJECTS(string ID, string YEARS, string WEEKS, string STORES,string NAMES, string ITEMS, string CONTENTS,string DAYS)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                            UPDATE [TKBUSINESS].[dbo].[TBPROJECTS]
                            SET 
                            [YEARS]=@YEARS
                            ,[WEEKS]=@WEEKS
                            ,[STORES]=@STORES
                            ,[NAMES]=@NAMES
                            ,[ITEMS]=@ITEMS
                            ,[CONTENTS]=@CONTENTS
                            ,[DAYS]=@DAYS
                            WHERE [ID]=@ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@YEARS", YEARS);
        m_db.AddParameter("@WEEKS", WEEKS);
        m_db.AddParameter("@STORES", STORES);
        m_db.AddParameter("@NAMES", NAMES);
        m_db.AddParameter("@ITEMS", ITEMS);
        m_db.AddParameter("@CONTENTS", CONTENTS);
        m_db.AddParameter("@DAYS", DAYS);




        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DELTBPROJECTS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE  [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

    public void ADDTBPROJECTS(string NEWWEEKS)
    {
        string YEARS = TextBox1.Text;
        string WEEKS = NEWWEEKS;
        string STORES = DropDownList1.SelectedValue.Trim();
        string NAMES = TextBox3.Text;
        string ITEMS = TextBox4.Text;
        string CONTENTS = TextBox5.Text;
        string DAYS = TextBox6.Text;


        if (!string.IsNullOrEmpty(YEARS) && !string.IsNullOrEmpty(WEEKS))
        {

            ADDTBPROJECTS(YEARS, WEEKS, STORES, NAMES, ITEMS, CONTENTS, DAYS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDTBPROJECTS(string YEARS, string WEEKS, string STORES, string NAMES, string ITEMS, string CONTENTS, string DAYS)
    {
        Label8.Text = "";

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        try
        {
            string cmdTxt = @"  
                            INSERT INTO  [TKBUSINESS].[dbo].[TBPROJECTS]
                            ([YEARS],[WEEKS],[STORES],[NAMES],[ITEMS],[CONTENTS],[DAYS])
                            VALUES
                            (@YEARS,@WEEKS,@STORES,@NAMES,@ITEMS,@CONTENTS,@DAYS)

                            ";



            m_db.AddParameter("@YEARS", YEARS);
            m_db.AddParameter("@WEEKS", WEEKS);
            m_db.AddParameter("@STORES", STORES);
            m_db.AddParameter("@NAMES", NAMES);
            m_db.AddParameter("@ITEMS", ITEMS);
            m_db.AddParameter("@CONTENTS", CONTENTS);
            m_db.AddParameter("@DAYS", DAYS);


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
