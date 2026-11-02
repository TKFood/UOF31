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


public partial class CDS_WebPage_TKRESEARCH_COSTDialogEDITDEL : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //不顯示子視窗的按鈕
        //((Master_DialogMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;
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
            BindDropDownList();

            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCH_TBCOSTRECORDS(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE(lblParam.Text);

        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        UPDATE(lblParam.Text);

        SEARCH_TBCOSTRECORDS(lblParam.Text);
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

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='人工記錄成本' ORDER BY  [ID]  ";

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

    public void SEARCH_TBCOSTRECORDS(string ID)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [MB001] AS '品號'
                        ,[MB002] AS '品名'
                        ,[MB003] AS '規格'
                        ,[COSTROW] AS '單位原料成本'
                        ,[COSTMAT] AS '單位物料成本'
                        ,[COSTHR] AS '單位人工成本'
                        ,[COSTMANU] AS '單位製造成本'
                        ,[COSTPRO] AS '單位加工成本'
                        ,([COSTROW]+[COSTMAT]+[COSTHR]+[COSTMANU]+[COSTPRO])  AS '單位成本'
                        ,[COMMEMTS] AS '備註'
                        ,[ISCLOSED] AS '是否結案'
                        FROM [TKRESEARCH].[dbo].[TBCOSTRECORDS]
                        WHERE MB001=@MB001
                        ORDER BY [MB001] 
      
                      
                        ";


        m_db.AddParameter("@MB001", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.Text = dt.Rows[0]["是否結案"].ToString();

            TextBox1.Text = dt.Rows[0]["品號"].ToString();
            TextBox2.Text = dt.Rows[0]["品名"].ToString();
            TextBox3.Text = dt.Rows[0]["規格"].ToString();
            TextBox4.Text = dt.Rows[0]["單位原料成本"].ToString();
            TextBox10.Text = dt.Rows[0]["單位物料成本"].ToString();
            TextBox5.Text = dt.Rows[0]["單位人工成本"].ToString();
            TextBox6.Text = dt.Rows[0]["單位製造成本"].ToString();
            TextBox7.Text = dt.Rows[0]["單位加工成本"].ToString();
            TextBox8.Text = dt.Rows[0]["單位成本"].ToString();
            TextBox9.Text = dt.Rows[0]["備註"].ToString();




        }




    }

    public void UPDATE(string ID)
    {
        string SERNO = lblParam.Text;
        string MB001 = TextBox1.Text;
        string MB002 = TextBox2.Text;
        string MB003 = TextBox3.Text;
        string COSTROW = TextBox4.Text;
        string COSTHR = TextBox5.Text;
        string COSTMANU = TextBox6.Text;
        string COSTPRO = TextBox7.Text;
        string COMMEMTS = TextBox9.Text;
        string ISCLOSED = DropDownList1.Text;
        string COSTMAT = TextBox10.Text;

        if (!string.IsNullOrEmpty(MB001))
        {
            UPDATE_TBCOSTRECORDS(
                        MB001, MB002, MB003, COSTROW, COSTMAT,COSTHR, COSTMANU, COSTPRO, COMMEMTS, ISCLOSED
                        );
        }

        Dialog.SetReturnValue2("REFRESH");
    }
    public void UPDATE_TBCOSTRECORDS(
                            string MB001, string MB002, string MB003, string COSTROW,string COSTMAT, string COSTHR, string COSTMANU, string COSTPRO, string COMMEMTS, string ISCLOSED


                                )
    {

        


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                       UPDATE [TKRESEARCH].[dbo].[TBCOSTRECORDS]
                        SET 
                        MB002=@MB002
                       ,MB003=@MB003
                     ,COSTROW=@COSTROW
                     ,COSTMAT=@COSTMAT
                     ,COSTHR=@COSTHR
                     ,COSTMANU=@COSTMANU
                     ,COSTPRO=@COSTPRO
                     ,COMMEMTS=@COMMEMTS
                     ,ISCLOSED=@ISCLOSED
              
                       
                        WHERE[MB001]=@MB001
                            ";

        m_db.AddParameter("@MB001", MB001);
        m_db.AddParameter("@MB002", MB002);
        m_db.AddParameter("@MB003", MB003);
        m_db.AddParameter("@COSTROW", COSTROW);
        m_db.AddParameter("@COSTMAT", COSTMAT);
        m_db.AddParameter("@COSTHR", COSTHR);
        m_db.AddParameter("@COSTMANU", COSTMANU);
        m_db.AddParameter("@COSTPRO", COSTPRO);
        m_db.AddParameter("@COMMEMTS", COMMEMTS);
        m_db.AddParameter("@ISCLOSED", ISCLOSED);


        m_db.ExecuteNonQuery(cmdTxt);


    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTBDEVNEW(lblParam.Text);
    }

    public void DELTBDEVNEW(string ID)
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"  DELETE [TKRESEARCH].[dbo].[TBDEVNEW]  WHERE [SERNO]=@ID
        //                    ";

        //m_db.AddParameter("@ID", ID);

        //m_db.ExecuteNonQuery(cmdTxt);


        //Dialog.SetReturnValue2("NeedPostBack");
        //Dialog.Close(this);
    }

    
    protected void CAL_TextBox8_TextChanged(object sender, EventArgs e)
    {
        decimal SUM = 0;
        SUM = SUM+Convert.ToDecimal(TextBox4.Text);
        SUM = SUM + Convert.ToDecimal(TextBox5.Text);
        SUM = SUM + Convert.ToDecimal(TextBox6.Text);
        SUM = SUM + Convert.ToDecimal(TextBox7.Text);
        SUM = SUM + Convert.ToDecimal(TextBox10.Text);

        TextBox8.Text = SUM.ToString();
    }
    #endregion


}
