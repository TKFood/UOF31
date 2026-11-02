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


public partial class CDS_WebPage_TKRESEARCHTBPROJECTDialogADDD : Ede.Uof.Utility.Page.BasePage
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

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='MEMOSTATUS' ORDER BY [PARAID] ";

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
        string PROJECTNO = TextBox1.Text.Trim();
        string PROJECTNAME = TextBox2.Text.Trim();
        string ONLINEFINISH = TextBox3.Text.Trim();
        string PAPERFINISH = TextBox4.Text.Trim();
        string MEMO = TextBox5.Text.Trim();
        string PROJECTSTATUS = DropDownList1.Text.Trim();


        if (!string.IsNullOrEmpty(PROJECTSTATUS) && !string.IsNullOrEmpty(PROJECTNO) && !string.IsNullOrEmpty(PROJECTNAME))
        {
            ADDTBSALESDEVMEMO(ID, PROJECTNO, PROJECTNAME, ONLINEFINISH, PAPERFINISH, MEMO, PROJECTSTATUS);
        }

        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);
    }
    public void ADDTBSALESDEVMEMO(Guid ID,string PROJECTNO, string PROJECTNAME, string ONLINEFINISH, string PAPERFINISH, string MEMO,string PROJECTSTATUS)
    {
       

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKRESEARCH].dbo.[TBPROJECT]
                        ([ID],[PROJECTNO],[PROJECTNAME],[ONLINEFINISH],[PAPERFINISH],[MEMO],[PROJECTSTATUS])
                        VALUES
                        (@ID,@PROJECTNO,@PROJECTNAME,@ONLINEFINISH,@PAPERFINISH,@MEMO,@PROJECTSTATUS)

                            ";


    
        m_db.AddParameter("@ID", ID);       
        m_db.AddParameter("@PROJECTNO", PROJECTNO);
        m_db.AddParameter("@PROJECTNAME", PROJECTNAME);
        m_db.AddParameter("@ONLINEFINISH", ONLINEFINISH);
        m_db.AddParameter("@PAPERFINISH", PAPERFINISH);
        m_db.AddParameter("@MEMO", MEMO);
        m_db.AddParameter("@PROJECTSTATUS", PROJECTSTATUS);

        m_db.ExecuteNonQuery(cmdTxt);

    }
    #endregion


}
