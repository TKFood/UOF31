using Ede.Uof.Utility.Page.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CDS_WebPage_Dialog : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //設定回傳值
        Dialog.SetReturnValue2("");

        //註冊Dialog的Button 狀態
        ((Master_DialogMasterPage)this.Master).Button1CausesValidation = false;
        ((Master_DialogMasterPage)this.Master).Button1AutoCloseWindow = false;
        ((Master_DialogMasterPage)this.Master).Button1OnClick += CDS_WebPage_Dialog_Button1OnClick;
  

        if (!IsPostBack)
        {
            //接收主頁面傳遞之參數
            lblParam.Text = Request["ID"];
            
        }
        
    }





    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        string YEARS = txtTextBox1.Text;
        string MONTHS = txtTextBox2.Text;
        string WEEKS = txtTextBox3.Text;
        string DATES = txtTextBox4.Text;
        string SELLER = txtTextBox5.Text;
        string MONEYS = txtTextBox6.Text;

        if (!string.IsNullOrEmpty(YEARS) && !string.IsNullOrEmpty(MONTHS) && !string.IsNullOrEmpty(WEEKS) && !string.IsNullOrEmpty(DATES) && !string.IsNullOrEmpty(SELLER) && !string.IsNullOrEmpty(MONEYS))
        {
            ADDTKSELLERMONEYS(YEARS, MONTHS, WEEKS, DATES, SELLER, MONEYS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);

    }





    #region
    public void ADDTKSELLERMONEYS(string YEARS, string MONTHS, string WEEKS, string DATES, string SELLER, string MONEYS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        var SQL = "INSERT INTO [UOFTEST].[dbo].[TKSELLERMONEYS] ([ID],[YEARS],[MONTHS],[WEEKS],[DATES],[SELLER],[MONEYS]) VALUES (@ID,@YEARS,@MONTHS,@WEEKS,@DATES,@SELLER,@MONEYS)";

        using (SqlConnection cnn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(SQL, cnn))
            {
                cmd.Parameters.AddWithValue("@ID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@YEARS",YEARS);
                cmd.Parameters.AddWithValue("@MONTHS",MONTHS);
                cmd.Parameters.AddWithValue("@WEEKS",WEEKS);
                cmd.Parameters.AddWithValue("@DATES",DATES);
                cmd.Parameters.AddWithValue("@SELLER",SELLER);
                cmd.Parameters.AddWithValue("@MONEYS",MONEYS);


                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
    #endregion


}
