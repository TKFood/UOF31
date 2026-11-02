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


public partial class CDS_WebPage_TBBU_TBSALESSCHEDULESDialogADD : Ede.Uof.Utility.Page.BasePage
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
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("PARAID", typeof(String));
    //    dt.Columns.Add("PARANAME", typeof(String));

    //    string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
    //    Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

    //    string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='MEMOSTATUS' ORDER BY [PARAID] ";

    //    dt.Load(m_db.ExecuteReader(cmdTxt));

    //    if (dt.Rows.Count > 0)
    //    {
    //        DropDownList1.DataSource = dt;
    //        DropDownList1.DataTextField = "PARANAME";
    //        DropDownList1.DataValueField = "PARANAME";
    //        DropDownList1.DataBind();

    //    }
    //    else
    //    {

    //    }

       
    }

    public void ADD()
    {
        string ID = "";
        string DEALER = TextBox1.Text;
        string OWNERS=TextBox2.Text;
        string EVENTS = TextBox3.Text;
        string MONTHS1 = TextBox4.Text;
        string MONTHS2 = TextBox5.Text;
        string MONTHS3 = TextBox6.Text;
        string MONTHS4 = TextBox7.Text;
        string MONTHS5 = TextBox8.Text;
        string MONTHS6 = TextBox9.Text;
        string MONTHS7 = TextBox10.Text;
        string MONTHS8 = TextBox11.Text;
        string MONTHS9 = TextBox12.Text;
        string MONTHS10 = TextBox13.Text;
        string MONTHS11 = TextBox14.Text;
        string MONTHS12 = TextBox15.Text;


        if (!string.IsNullOrEmpty(DEALER))
        {
            ADDTTBSALESSCHEDULES(ID, DEALER, OWNERS, EVENTS, MONTHS1, MONTHS2, MONTHS3, MONTHS4, MONTHS5, MONTHS6, MONTHS7, MONTHS8, MONTHS9, MONTHS10, MONTHS11, MONTHS12);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDTTBSALESSCHEDULES(string ID, string DEALER, string OWNERS, string EVENTS, string MONTHS1, string MONTHS2, string MONTHS3, string MONTHS4, string MONTHS5, string MONTHS6, string MONTHS7, string MONTHS8, string MONTHS9, string MONTHS10, string MONTHS11, string MONTHS12)
    {
       
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        INSERT INTO  [TKBUSINESS].[dbo].[TBSALESSCHEDULES]
                        ([DEALER],[OWNERS],[EVENTS],[MONTHS1],[MONTHS2],[MONTHS3],[MONTHS4],[MONTHS5],[MONTHS6],[MONTHS7],[MONTHS8],[MONTHS9],[MONTHS10],[MONTHS11],[MONTHS12])
                        VALUES
                        (@DEALER,@OWNERS,@EVENTS,@MONTHS1,@MONTHS2,@MONTHS3,@MONTHS4,@MONTHS5,@MONTHS6,@MONTHS7,@MONTHS8,@MONTHS9,@MONTHS10,@MONTHS11,@MONTHS12)
                            ";


    
        m_db.AddParameter("@ID", "");
        m_db.AddParameter("@DEALER", DEALER);
        m_db.AddParameter("@OWNERS", OWNERS);
        m_db.AddParameter("@EVENTS", EVENTS);
        m_db.AddParameter("@MONTHS1", MONTHS1);
        m_db.AddParameter("@MONTHS2", MONTHS2);
        m_db.AddParameter("@MONTHS3", MONTHS3);
        m_db.AddParameter("@MONTHS4", MONTHS4);
        m_db.AddParameter("@MONTHS5", MONTHS5);
        m_db.AddParameter("@MONTHS6", MONTHS6);
        m_db.AddParameter("@MONTHS7", MONTHS7);
        m_db.AddParameter("@MONTHS8", MONTHS8);
        m_db.AddParameter("@MONTHS9", MONTHS9);
        m_db.AddParameter("@MONTHS10", MONTHS10);
        m_db.AddParameter("@MONTHS11", MONTHS11);
        m_db.AddParameter("@MONTHS12", MONTHS12);



        m_db.ExecuteNonQuery(cmdTxt);



        


    }
    #endregion


}
