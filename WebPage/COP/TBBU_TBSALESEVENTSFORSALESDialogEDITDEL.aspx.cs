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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CDS_WebPage_TBBU_TBSALESEVENTSFORSALESDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ACCOUNT = null;
        string NAME = null;
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        //設定回傳值
        Dialog.SetReturnValue2("");

        //註冊Dialog的Button 狀態
        ((Master_DialogMasterPage)this.Master).Button1CausesValidation = false;
        ((Master_DialogMasterPage)this.Master).Button1AutoCloseWindow = false;
        ((Master_DialogMasterPage)this.Master).Button1OnClick += CDS_WebPage_Dialog_Button1OnClick;
        ((Master_DialogMasterPage)this.Master).Button2OnClick += Button2OnClick;

        if (!IsPostBack)
        {
            BindDropDownList1(NAME);
            BindDropDownList2();
            BindDropDownList3();

            //接收主頁面傳遞之參數
            lblParam.Text = Request["ID"];


            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHTBSALESEVENTS(lblParam.Text);
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

        SEARCHTBSALESEVENTS(lblParam.Text);
    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBSALESEVENTS(lblParam.Text);
    }


    #endregion

    #region FUNCTION
    private void BindDropDownList1(string NAME)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("NAME", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);


        string cmdTxt = @"SELECT  [ID],[NAME] FROM [TKBUSINESS].[dbo].[TBSALESNAME] WHERE ([NAME]=@NAME OR [LEADER] LIKE '%'+@NAME+'%'  ) ORDER BY [ID]";

        m_db.AddParameter("@NAME", NAME);

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "NAME";
            DropDownList1.DataValueField = "NAME";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("KINDS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[KINDS] FROM [TKBUSINESS].[dbo].[TBSALESKINDS] ORDER BY [ID]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "KINDS";
            DropDownList2.DataValueField = "KINDS";
            DropDownList2.DataBind();

        }
        else
        {

        }



    }
    private void BindDropDownList3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ISCLOSES", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT [ID],[ISCLOSES] FROM [TKBUSINESS].[dbo].[TBSALESCLOSES] ORDER BY [ID]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList3.DataSource = dt;
            DropDownList3.DataTextField = "ISCLOSES";
            DropDownList3.DataValueField = "ISCLOSES";
            DropDownList3.DataBind();

        }
        else
        {

        }



    }
    private void BindDropDownList()
    {
      



    }

    public void SEARCHTBSALESEVENTS(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   
                       SELECT 
                        [ID]
                        ,[SALES]
                        ,[KINDS]
                        ,[CLIENTS]
                        ,[PROJECTS]
                        ,[EVENTS]
                        ,[SDAYS]
                        ,[EDAYS]
                        ,[COMMENTS]
                        ,[ISCLOSE]
                        FROM [TKBUSINESS].[dbo].[TBSALESEVENTS]
                        WHERE [ID]=@ID
                        ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.Text = dt.Rows[0]["SALES"].ToString();
            DropDownList2.Text = dt.Rows[0]["KINDS"].ToString();
            TextBox1.Text = dt.Rows[0]["CLIENTS"].ToString();
            TextBox2.Text = dt.Rows[0]["PROJECTS"].ToString();
            TextBox3.Text = dt.Rows[0]["EVENTS"].ToString();
            RadDatePicker1.SelectedDate = Convert.ToDateTime(dt.Rows[0]["SDAYS"].ToString());
            RadDatePicker2.SelectedDate = Convert.ToDateTime(dt.Rows[0]["EDAYS"].ToString());  
            TextBox6.Text = dt.Rows[0]["COMMENTS"].ToString();
            DropDownList3.Text = dt.Rows[0]["ISCLOSE"].ToString();


        }




    }

    public void UPDATE()
    {
        string ID = lblParam.Text;
        string SALES = DropDownList1.Text;
        string KINDS = DropDownList2.Text;       
        string CLIENTS = TextBox1.Text;
        string PROJECTS= TextBox2.Text;
        string EVENTS = TextBox3.Text;
        string SDAYS = RadDatePicker1.SelectedDate.Value.ToString("yyyy/MM/dd");
        string EDAYS = RadDatePicker2.SelectedDate.Value.ToString("yyyy/MM/dd");
        string COMMENTS = TextBox6.Text;
        string ISCLOSE = DropDownList3.Text;





        if (!string.IsNullOrEmpty(ID) )
        {
            UPDATETBSALESEVENTS(ID, SALES, KINDS, CLIENTS, PROJECTS, EVENTS, SDAYS, EDAYS, COMMENTS, ISCLOSE);
        }

        Dialog.SetReturnValue2("NeedPostBack");
    }
    public void UPDATETBSALESEVENTS(string ID, string SALES, string KINDS,string CLIENTS,string PROJECTS, string EVENTS, string SDAYS, string EDAYS, string COMMENTS, string ISCLOSE)
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                           UPDATE  [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            SET 
                            [SALES]=@SALES
                            ,[KINDS]=@KINDS
                            ,[CLIENTS]=@CLIENTS
                            ,[PROJECTS]=@PROJECTS
                            ,[EVENTS]=@EVENTS
                            ,[SDAYS]=@SDAYS
                            ,[EDAYS]=@EDAYS
                            ,[COMMENTS]=@COMMENTS
                            ,[ISCLOSE]=@ISCLOSE
                            WHERE [ID]=@ID
                   
                            ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@SALES", SALES);
        m_db.AddParameter("@KINDS", KINDS);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@PROJECTS", PROJECTS);
        m_db.AddParameter("@EVENTS", EVENTS);
        m_db.AddParameter("@SDAYS", SDAYS);
        m_db.AddParameter("@EDAYS", EDAYS);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@ISCLOSE", ISCLOSE);




        m_db.ExecuteNonQuery(cmdTxt);



    }



    public void DELTBSALESEVENTS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  DELETE  [TKBUSINESS].[dbo].[TBSALESEVENTS] WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);


        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }

    [WebMethod]
    public static AutoCompleteBoxData GetCompanyNames(object context)
    {
        string searchString = ((Dictionary<string, object>)context)["Text"].ToString();

        List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

        DataTable data = SEARCHCOMPANYNAME(searchString);

        //string CLIENTS = RadAutoCompleteBox1.Text;

        foreach (DataRow row in data.Rows)
        {
            AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
            childNode.Text = row["COMPANY_NAME"].ToString();
            childNode.Value = row["COMPANY_NAME"].ToString();
            result.Add(childNode);
        }

        AutoCompleteBoxData res = new AutoCompleteBoxData();
        res.Items = result.ToArray();

        return res;

        //string searchString = ((Dictionary<string, object>)context)["Text"].ToString();
        //DataTable data = GetChildNodes(searchString);
        //List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

        //foreach (DataRow row in data.Rows)
        //{
        //    AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
        //    childNode.Text = row["ACCOUNT"].ToString();
        //    childNode.Value = row["ACCOUNT"].ToString();
        //    result.Add(childNode);
        //}

        //AutoCompleteBoxData res = new AutoCompleteBoxData();
        //res.Items = result.ToArray();

        //return res;
    }


    private static DataTable SEARCHCOMPANYNAME(string searchString)
    {
        DataTable dt = new DataTable();



        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                            SELECT  
                            [COMPANY_ID]
                            ,[COMPANY_NAME]
                            FROM [HJ_BM_DB].[dbo].[tb_COMPANY]
                            WHERE [COMPANY_NAME] LIKE @COMPANY_NAME+'%'
                            ORDER BY [COMPANY_NAME]
                                
                            ");

        m_db.AddParameter("@COMPANY_NAME", searchString);

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt;

        }
        else
        {
            return null;
        }
    }

    #endregion


}
