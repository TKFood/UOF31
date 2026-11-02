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
            if(!string.IsNullOrEmpty(lblParam.Text))
            {
                SEARCHTKSELLERMONEYS(lblParam.Text);
            }
            
        }
        
    }





    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);
        string ID = lblParam.Text;
        string YEARS = txtTextBox1.Text;
        string MONTHS = txtTextBox2.Text;
        string WEEKS = txtTextBox3.Text;
        string DATES = txtTextBox4.Text;
        string SELLER = txtTextBox5.Text;
        string MONEYS = txtTextBox6.Text;

        if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(YEARS) && !string.IsNullOrEmpty(MONTHS) && !string.IsNullOrEmpty(WEEKS) && !string.IsNullOrEmpty(DATES) && !string.IsNullOrEmpty(SELLER) && !string.IsNullOrEmpty(MONEYS))
        {
            UPDATETKSELLERMONEYS(ID,YEARS, MONTHS, WEEKS, DATES, SELLER, MONEYS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);

    }





    #region
    public void SEARCHTKSELLERMONEYS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("SELECT  [ID],[YEARS] ,[MONTHS],[WEEKS],CONVERT(NVARCHAR,[DATES],112) AS DATES,[SELLER],[MONEYS] FROM [UOFTEST].[dbo].[TKSELLERMONEYS]  WHERE [ID]=@ID", conn);
            command.Parameters.AddWithValue("@ID", ID);

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if(ds.Tables[0].Rows.Count>0)
            {
                txtTextBox1.Text = ds.Tables[0].Rows[0]["YEARS"].ToString();
                txtTextBox2.Text = ds.Tables[0].Rows[0]["MONTHS"].ToString();
                txtTextBox3.Text = ds.Tables[0].Rows[0]["WEEKS"].ToString();
                txtTextBox4.Text = ds.Tables[0].Rows[0]["DATES"].ToString();
                txtTextBox5.Text = ds.Tables[0].Rows[0]["SELLER"].ToString();
                txtTextBox6.Text = ds.Tables[0].Rows[0]["MONEYS"].ToString();
            }
        }
        
  
    }

    public void UPDATETKSELLERMONEYS(string ID,string YEARS, string MONTHS, string WEEKS, string DATES, string SELLER, string MONEYS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        var SQL = "UPDATE [UOFTEST].[dbo].[TKSELLERMONEYS] SET [YEARS]=@YEARS,[MONTHS]=@MONTHS,[WEEKS]=@WEEKS,[DATES]=@DATES,[SELLER]=@SELLER,[MONEYS]=@MONEYS WHERE[ID]=@ID";

        using (SqlConnection cnn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(SQL, cnn))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@YEARS", YEARS);
                cmd.Parameters.AddWithValue("@MONTHS", MONTHS);
                cmd.Parameters.AddWithValue("@WEEKS", WEEKS);
                cmd.Parameters.AddWithValue("@DATES", DATES);
                cmd.Parameters.AddWithValue("@SELLER", SELLER);
                cmd.Parameters.AddWithValue("@MONEYS", MONEYS);


                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        DELTKSELLERMONEYS(lblParam.Text);
    }

    public void DELTKSELLERMONEYS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        var SQL = "DELETE [UOFTEST].[dbo].[TKSELLERMONEYS]  WHERE[ID]=@ID";

        using (SqlConnection cnn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(SQL, cnn))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    #endregion


}
