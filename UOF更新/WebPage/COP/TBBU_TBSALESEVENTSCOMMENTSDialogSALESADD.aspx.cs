using Ede.Uof.Utility.Component;
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
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CDS_WebPage_TBBU_TBSALESEVENTSCOMMENTSDialogSALESADD : Ede.Uof.Utility.Page.BasePage
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
            //SEARCHTBSALESEVENTS(lblParam.Text);

            if (!string.IsNullOrEmpty(lblParam.Text))
            {                
                BindGrid(lblParam.Text);
                //SEARCHTBSALESDEVMEMO(lblParam.Text);
            }

        }
        
    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);
        if(!string.IsNullOrEmpty(lblParam.Text) && !string.IsNullOrEmpty(TextBox1.Text) )
        {
            ADDTBSALESEVENTSCOMMENTS(lblParam.Text, TextBox1.Text, LabelNAME.Text);
            UPDATETBSALESEVENTS(lblParam.Text, TextBox1.Text, LabelNAME.Text);

            ADD_HJ_BM_DB_tb_NOTE(lblParam.Text, TextBox1.Text, LabelNAME.Text);
        }
        
        Dialog.SetReturnValue2("NeedPostBack");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        if (!string.IsNullOrEmpty(lblParam.Text) && !string.IsNullOrEmpty(TextBox1.Text))
        {
            ADDTBSALESEVENTSCOMMENTS(lblParam.Text, TextBox1.Text, LabelNAME.Text);
            UPDATETBSALESEVENTS(lblParam.Text, TextBox1.Text, LabelNAME.Text);

            ADD_HJ_BM_DB_tb_NOTE(lblParam.Text, TextBox1.Text, LabelNAME.Text);
        }

        BindGrid(lblParam.Text);

        Dialog.SetReturnValue2("NeedPostBack");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //將上傳的檔案確認寫入到SERVER
        var imgSavePath = "";//儲存圖片路徑       
        var result = UPLOAD(ref imgSavePath);
        if (result)
        {
            //圖片上傳完成  進行寫資料庫操作
            Response.Write("<script>alert('已儲存')</script>");

        }
        else
        {
            Response.Write("<script>alert('已上傳過1次，或上傳失敗')</script>");
        }

    }
    #endregion


    #region FUNCTION
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string PATH = "https://eip.tkfood.com.tw/BM/upload/note/";
        Image img = (Image)e.Row.FindControl("Image1");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            Image img1 = (Image)e.Row.FindControl("Image1");

            if (!string.IsNullOrEmpty(row["FILENAME"].ToString()))
            {
                img.ImageUrl = PATH + row["FILENAME"].ToString();

                //獲取當前行的圖片路徑
                string ImgUrl = img.ImageUrl;
                ////給帶圖片的單元格添加點擊事件
                //e.Row.Cells[10].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
                //    + ".checked=true;CellClick('" + ImgUrl + "')");

                //img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
            }


        }

      
    }

    public bool UPLOAD(ref string imgSavePath)
    {
        string STATUS = "N";
        //獲取上傳的檔名
        string FILENAME = this.FileUpload.FileName;
        FILENAME = DateTime.Now.ToString("yyyyMMddHHmmss") + FILENAME;
        //你的伺服器地址
        string route = "https://eip.tkfood.com.tw/BM/UPLOAD";

        //如果伺服器不存在該名資料夾 就生成一個
        //if (!Directory.Exists(route + "/UPLOAD/"))
        //{
        //    Directory.CreateDirectory(route + "/UPLOAD/");
        //}

        //獲取物理路徑（圖片儲存的位置）
        //上傳圖片時，雖然是傳到「C:\VSPROJECT\TKUOF\UOF18\UPLOAD」
        //但是在主機上，UPLOAD的資料夾是指到[ https://eip.tkfood.com.tw/BM/upload/note/] 中
        string PATH = Server.MapPath("~/UPLOAD/");
        //UPLOADTEMP是存在TKUOF備查的
        string PATH2 = Server.MapPath("~/UPLOADTEMP/");
        //string path = Server.MapPath(@"\../HJ_BM/UPLOAD");



        //判斷上傳控制元件是否上傳檔案
        if (FileUpload.HasFile && LabelISSTATUS.Text.Equals("N"))
        {
            //判斷上傳檔案的副檔名是否為允許的副檔名".gif", ".png", ".jpeg", ".jpg" ,".bmp"
            String fileExtension = System.IO.Path.GetExtension(FILENAME).ToLower();
            String[] Extensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (int i = 0; i < Extensions.Length; i++)
            {
                if (fileExtension == Extensions[i])
                {
                    //進行上傳圖片操作
                    this.FileUpload.PostedFile.SaveAs(PATH + FILENAME);
                    this.FileUpload.PostedFile.SaveAs(PATH2 + FILENAME);
                    imgSavePath = PATH + FILENAME;//原圖儲存路徑



                    LabelNAME.Text = FILENAME;
                    Label14.Text = "已上傳";
                    LabelISSTATUS.Text = "Y"; ;

                    return true;
                }
            }
        }
        return false;
    }

    private void BindGrid(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                           SELECT 
                            [ID]
                            ,[MID]
                            ,CONVERT(NVARCHAR, [ADDDATES],120) AS ADDDATES 
                            ,REPLACE([COMMENTS],char(10),'<br/>') AS [COMMENTS] 
                            ,[FILENAME]
                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTSCOMMENTS]
                            WHERE [MID]=@ID     
                            ORDER BY [ADDDATES] DESC
                            ";

        m_db.AddParameter("@ID", ID);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();




    }

    public void SEARCHTBSALESEVENTS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT [ID]
                        ,[SALES]
                        ,[KINDS]
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
            TextBox2.Text = dt.Rows[0]["EVENTS"].ToString();
        }


    }

    public void ADDTBSALESEVENTSCOMMENTS(string MID, string COMMENTS,string FILENAME)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO  [TKBUSINESS].[dbo].[TBSALESEVENTSCOMMENTS]
                        ([MID],[ADDDATES],[COMMENTS],[FILENAME])
                        VALUES
                        (@MID,@ADDDATES,@COMMENTS,@FILENAME)
                            ";

       
        
        m_db.AddParameter("@MID", MID);
        m_db.AddParameter("@ADDDATES", Convert.ToDateTime(DateTime.Now));
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@FILENAME", FILENAME);

        m_db.ExecuteNonQuery(cmdTxt);

        
    }

    public void UPDATETBSALESEVENTS(string ID, string COMMENTS,string FILENAME)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  UPDATE [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            SET [COMMENTS]=@COMMENTS
                            ,[FILENAME]=@FILENAME
                            WHERE [ID]=@ID
                            ";

        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@FILENAME", FILENAME);


        m_db.ExecuteNonQuery(cmdTxt);

        
    }

    public string SEARCHPROJECTSSALES(string ID)
    {
        DataTable dt = new DataTable();


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
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
                            WHERE ID=@ID
                          
                                ");

        m_db.AddParameter("@ID", ID);

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["SALES"].ToString();

        }
        else
        {
            return null;
        }
    }

    public string SEARCHPROJECTSCLIENTS(string ID)
    {
        DataTable dt = new DataTable();


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
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
                            WHERE ID=@ID
                          
                                ");

        m_db.AddParameter("@ID", ID);

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["CLIENTS"].ToString();

        }
        else
        {
            return null;
        }
    }

    public string SEARCHPROJECTSCOMMENTS(string ID, string COMMENTS)
    {
        DataTable dt = new DataTable();
        string NOTE_CONTENT = null;

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
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
                            WHERE ID=@ID
                          
                                ");

        m_db.AddParameter("@ID", ID);

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));
        
        string KIND = null;
        
        if (!string.IsNullOrEmpty(dt.Rows[0]["KINDS"].ToString()))
        {
            KIND = dt.Rows[0]["KINDS"].ToString();
        }
            

        if (dt.Rows.Count > 0)
        {         

            if (!string.IsNullOrEmpty(dt.Rows[0]["PROJECTS"].ToString()))
            {
                NOTE_CONTENT = NOTE_CONTENT + dt.Rows[0]["PROJECTS"].ToString() + "<br>";
            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["EVENTS"].ToString()))
            {
                NOTE_CONTENT = NOTE_CONTENT + dt.Rows[0]["EVENTS"].ToString() + "<br>";
            }

            if (KIND.Equals("拜訪"))
            {

                if (!string.IsNullOrEmpty(dt.Rows[0]["EDAYS"].ToString()))
                {
                    NOTE_CONTENT = NOTE_CONTENT + "拜訪日:" + dt.Rows[0]["EDAYS"].ToString() + "<br>";
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(dt.Rows[0]["EDAYS"].ToString()))
                {
                    NOTE_CONTENT = NOTE_CONTENT + "結案日:" + dt.Rows[0]["EDAYS"].ToString() + "<br>";
                }
            }
            if (!string.IsNullOrEmpty(COMMENTS))
            {
                NOTE_CONTENT = NOTE_CONTENT + COMMENTS + "";
            }

            return NOTE_CONTENT;
        }
        else
        {
            return null;
        }
    }

    public void ADD_HJ_BM_DB_tb_NOTE(string ID, string COMMENTS,string FILENAME)
    {
        string NOTE_ID = null;
        string NOTE_CONTENT = SEARCHPROJECTSCOMMENTS(ID, COMMENTS);
        string NOTE_KIND = "1";
        string FILE_NAME = FILENAME;
        string NOTE_DATE = DateTime.Now.ToString("yyyy-MM-dd");
        string NOTE_TIME = DateTime.Now.ToString("HH:mm");
        string UPDATE_DATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
        string CONTACT_ID = "0";
        string COMPANY_ID = SEARCHCOMPANY_ID(SEARCHPROJECTSCLIENTS(ID));
        string OPPORTUNITY_ID = "0";
        string SALES_STAGE = null;
        string CREATE_DATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string CREATE_USER_ID = SEARCHCREATE_USER_ID(SEARCHPROJECTSSALES(ID));



        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        if (!String.IsNullOrEmpty(COMPANY_ID) && !String.IsNullOrEmpty(CREATE_USER_ID))
        {
            try
            {
                string cmdTxt = @"  
                                INSERT INTO [HJ_BM_DB].[dbo].[tb_NOTE]
                                (                            
                                [NOTE_CONTENT]
                                ,[NOTE_KIND]
                                ,[FILE_NAME]                               
                                ,[NOTE_DATE]
                                ,[NOTE_TIME]
                                ,[UPDATE_DATETIME]
                                ,[CONTACT_ID]
                                ,[COMPANY_ID]
                                ,[OPPORTUNITY_ID]
                               
                                ,[CREATE_DATETIME]
                                ,[CREATE_USER_ID]
                                )
                                VALUES
                                (                              
                                @NOTE_CONTENT
                                ,@NOTE_KIND
                                ,@FILE_NAME]      
                                ,@NOTE_DATE
                                ,@NOTE_TIME
                                ,@UPDATE_DATETIME
                                ,@CONTACT_ID
                                ,@COMPANY_ID
                                ,@OPPORTUNITY_ID
                               
                                ,@CREATE_DATETIME
                                ,@CREATE_USER_ID
                                )
                                 
                                ";




                m_db.AddParameter("@NOTE_CONTENT", NOTE_CONTENT);
                m_db.AddParameter("@NOTE_KIND", NOTE_KIND);
                m_db.AddParameter("@FILE_NAME", FILE_NAME);
                m_db.AddParameter("@NOTE_DATE", NOTE_DATE);
                m_db.AddParameter("@NOTE_TIME", NOTE_TIME);
                m_db.AddParameter("@UPDATE_DATETIME", UPDATE_DATETIME);
                m_db.AddParameter("@CONTACT_ID", CONTACT_ID);
                m_db.AddParameter("@COMPANY_ID", COMPANY_ID);
                m_db.AddParameter("@OPPORTUNITY_ID", OPPORTUNITY_ID);
                //m_db.AddParameter("@SALES_STAGE", SALES_STAGE);
                m_db.AddParameter("@CREATE_DATETIME", CREATE_DATETIME);
                m_db.AddParameter("@CREATE_USER_ID", CREATE_USER_ID);





                m_db.ExecuteNonQuery(cmdTxt);


            }
            catch
            {

            }
            finally
            {

            }
        }


    }

    public string SEARCHCOMPANY_ID(string COMPANYNAME)
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
                            WHERE [COMPANY_NAME]=@COMPANY_NAME
                                ");

        m_db.AddParameter("@COMPANY_NAME", COMPANYNAME);

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["COMPANY_ID"].ToString();

        }
        else
        {
            return null;
        }
    }

    public string SEARCHCREATE_USER_ID(string USER_NAME)
    {
        DataTable dt = new DataTable();


        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                            SELECT 
                            [USER_ID]
                            ,[USER_NAME]
                            FROM [HJ_BM_DB].[dbo].[tb_USER]
                            WHERE [USER_NAME]=@USER_NAME
                                ");

        m_db.AddParameter("@USER_NAME", USER_NAME);

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["USER_ID"].ToString();

        }
        else
        {
            return null;
        }
    }

    #endregion


}
