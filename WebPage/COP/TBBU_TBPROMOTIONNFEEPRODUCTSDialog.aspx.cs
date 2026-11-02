using Ede.Uof.Utility.Component;
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



public partial class CDS_WebPage_TBBU_TBPROMOTIONNFEEPRODUCTSDialog : Ede.Uof.Utility.Page.BasePage
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
                SEARCHTBSALESDEVMEMO(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);
        if(!string.IsNullOrEmpty(lblParam.Text)&& !string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox3.Text) )
        {
            string MID = lblParam.Text;
            string MB001 = TextBox1.Text.Trim();
            string MB002= TextBox3.Text.Trim();
            string MB003= TextBox4.Text.Trim();
            string MB004= TextBox5.Text.Trim();
            string PRICES= TextBox6.Text.Trim();
            string SPRICES= TextBox7.Text.Trim();
            string COSTS = TextBox8.Text.Trim();
            string NUMS= TextBox9.Text.Trim();
            string MONEYS= TextBox10.Text.Trim();
            string FEES= TextBox11.Text.Trim();
            string EARNS= TextBox12.Text.Trim();

            ADDTBPROMOTIONNFEEPRODUCTS(MID, MB001, MB002, MB003, MB004, PRICES, SPRICES, COSTS, NUMS, MONEYS, FEES, EARNS);

            UPDATETBPROMOTIONNFEE(MID);
        }
        
        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        if (!string.IsNullOrEmpty(lblParam.Text) && !string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox3.Text))
        {
            string MID = lblParam.Text;
            string MB001 = TextBox1.Text.Trim();
            string MB002 = TextBox3.Text.Trim();
            string MB003 = TextBox4.Text.Trim();
            string MB004 = TextBox5.Text.Trim();
            string PRICES = TextBox6.Text.Trim();
            string SPRICES = TextBox7.Text.Trim();
            string COSTS = TextBox8.Text.Trim();
            string NUMS = TextBox9.Text.Trim();
            string MONEYS = TextBox10.Text.Trim();
            string FEES = TextBox11.Text.Trim();
            string EARNS = TextBox12.Text.Trim();

            ADDTBPROMOTIONNFEEPRODUCTS(MID, MB001, MB002, MB003, MB004, PRICES, SPRICES, COSTS, NUMS, MONEYS, FEES, EARNS);

            UPDATETBPROMOTIONNFEE(MID);
        }

        BindGrid(lblParam.Text);

        Dialog.SetReturnValue2("NeedPostBack");
    }


    #endregion


    #region FUNCTION



    private void BindGrid(string ID)
    {
        StringBuilder cmdTxt = new StringBuilder();

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        cmdTxt.AppendFormat(@" 
                            SELECT 
                            [ID]
                            ,[MID]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MB004]
                            ,[PRICES]
                            ,[SPRICES]
                            ,[COSTS]
                            ,[NUMS]
                            ,[MONEYS]
                            ,[FEES]
                            ,[EARNS]
                            FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS]
                            WHERE [MID]=@MID
                            ");

        m_db.AddParameter("@MID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();




    }

    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    //Button1
        //    //Get the button that raised the event
        //    Button btn = (Button)e.Row.FindControl("Button1");

        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //    //string cellvalue = gvr.Cells[2].Text.Trim();
        //    string Cellvalue = btn.CommandArgument;

        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    Button lbtnName = (Button)e.Row.FindControl("Button1");

        //    ExpandoObject param = new { ID = Cellvalue }.ToExpando();

        //    //Grid開窗是用RowDataBound事件再開窗
        //    //Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBPROMOTIONNFEEDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

        //    DELTETBPROMOTIONNFEEDETAILS(Cellvalue);

        //}

      

    }
    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string MID = lblParam.Text;
        string ID = "";

        if (e.CommandName == "Del")
        {
            ID = (e.CommandArgument.ToString());
            //-- your delete method here
            DELETETBPROMOTIONNFEEPRODUCTS(MID, ID);
            UPDATETBPROMOTIONNFEE(MID);

            BindGrid(lblParam.Text);
        }
    }
    public void SEARCHTBSALESDEVMEMO(string ID)
    {
        StringBuilder cmdTxt = new StringBuilder();

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        cmdTxt.AppendFormat(@" 
                            SELECT 
                            [ID]
                            ,[YEARS]
                            ,[NAMES]
                            FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEE]
                            WHERE [ID]=@ID
                            ");

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            TextBox2.Text = dt.Rows[0]["NAMES"].ToString();
        }

        
    }

    public void ADDTBPROMOTIONNFEEPRODUCTS(string MID, string MB001, string MB002, string MB003, string MB004, string PRICES, string SPRICES, string COSTS, string NUMS, string MONEYS, string FEES, string EARNS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        INSERT INTO [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS]
                        ([MID],[MB001],[MB002],[MB003],[MB004],[PRICES],[SPRICES],[COSTS],[NUMS],[MONEYS],[FEES],[EARNS])
                        VALUES
                        (@MID,@MB001,@MB002,@MB003,@MB004,@PRICES,@SPRICES,@COSTS,@NUMS,@MONEYS,@FEES,@EARNS)
                            ";

       
        m_db.AddParameter("@MID", MID);
        m_db.AddParameter("@MB001", MB001);
        m_db.AddParameter("@MB002", MB002);
        m_db.AddParameter("@MB003", MB003);
        m_db.AddParameter("@MB004", MB004);
        m_db.AddParameter("@PRICES", PRICES);
        m_db.AddParameter("@SPRICES", SPRICES);
        m_db.AddParameter("@COSTS", COSTS);
        m_db.AddParameter("@NUMS", NUMS);
        m_db.AddParameter("@MONEYS", MONEYS);
        m_db.AddParameter("@FEES", FEES);
        m_db.AddParameter("@EARNS", EARNS);

        m_db.ExecuteNonQuery(cmdTxt);

        int i = 0;

        
    }

    public void DELETETBPROMOTIONNFEEPRODUCTS(string MID, string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                            DELETE [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS]
                            WHERE MID=@MID AND ID=@ID
                            ";

        m_db.AddParameter("@MID", MID);
        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);

        
    }

    public void UPDATETBPROMOTIONNFEE(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKBUSINESS].[dbo].[TBPROMOTIONNFEE]
                        SET [SALESNUMS]=(SELECT SUM([NUMS]) FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS] WHERE [MID]=@ID)
                        ,[SALESMONEYS]=(SELECT SUM(MONEYS) FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS] WHERE [MID]=@ID)
                        ,[PROFITS]=(SELECT ISNULL(SUM(MONEYS-COSTS-FEES),0) FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS] WHERE [MID]=@ID)-(SELECT ISNULL(SUM([FEEMONEYS]),0) FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEDETAILS] WHERE [MID]=@ID)
                        ,[COSTMONEYS]=(SELECT ISNULL(SUM(COSTS),0) FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS] WHERE [MID]=@ID)
                        ,[FEEMONEYS]=(SELECT ISNULL(SUM(FEES),0) FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS] WHERE [MID]=@ID)+(SELECT ISNULL(SUM([FEEMONEYS]),0) FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEDETAILS] WHERE [MID]=@ID)
                        WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    #endregion


}
