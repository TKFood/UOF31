using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Ede.Uof.WKF.Design;
using System.Collections.Generic;
using Ede.Uof.WKF.Utility;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.WKF.Design.Data;
using Ede.Uof.WKF.VersionFields;
using System.Xml;
using System.Linq;
using System.Data.SqlClient;
using Ede.Uof.Utility.Data;
using System.Xml.Linq;
using System.Text;

public partial class WKF_OptionalFields_TKUOFtb_COMPANYCOMPANY_ID : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
{

	#region ==============公開方法及屬性==============
    //表單設計時
	//如果為False時,表示是在表單設計時
    private bool m_ShowGetValueButton = true;
    public bool ShowGetValueButton
    {
        get { return this.m_ShowGetValueButton; }
        set { this.m_ShowGetValueButton = value; }
    }

    #endregion

    string MA001;
    string MA002;
    DataSet dsCOMPANY=new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
		//這裡不用修改
		//欄位的初始化資料都到SetField Method去做
        SetField(m_versionField);

        if (!IsPostBack)
        {
            BindDropDownList();
            BindDropDownList2();
            BindDropDownList3();
            BindDropDownList4();
            BindDropDownList5();
            BindDropDownList6();
            BindDropDownList7();
            BindDropDownList8();
            BindDropDownList9();
            BindDropDownList10();
            BindDropDownList11();
            BindDropDownList12();
            BindDropDownList13();
            BindDropDownList14();
            BindDropDownList15();

        }
    }    

    /// <summary>
    /// 外掛欄位的條件值
    /// </summary>
    public override string ConditionValue
    {
        get
        {
            //回傳字串
            //此字串的內容將會被表單拿來當做條件判斷的值
            //return String.Empty;
            return TextBox1.Text;
        }
    }

    /// <summary>
    /// 是否被修改
    /// </summary>
    public override bool IsModified
    {
        get
        {
			//請自行判斷欄位內容是否有被修改
			//有修改回傳True
			//沒有修改回傳False
            //若實作產品標準的控制修改權限必需實作
            //一般是用 m_versionField.FieldValue (表單開啟前的值)
            //      和this.FieldValue (當前的值) 作比對
			return false;
        }
    }

    /// <summary>
    /// 查詢顯示的標題
    /// </summary>
    public override string DisplayTitle
    {
        get
        {
            //表單查詢或WebPart顯示的標題
            //回傳字串
            return TextBox1.Text;
        }
    }

    /// <summary>
    /// 訊息通知的內容
    /// </summary>
    public override string Message
    {
        get
        {
            //表單訊息通知顯示的內容
            //回傳字串
            return TextBox1.Text;
        }
    }


    /// <summary>
    /// 真實的值
    /// </summary>
    public override string RealValue
    {
        get
        {
            //回傳字串
            //取得表單欄位簽核者的UsetSet字串
            //內容必須符合EB UserSet的格式
            return TextBox1.Text;
        }
        set
        {
			//這個屬性不用修改
            base.m_fieldValue = value;
        }
    }


    /// <summary>
    /// 欄位的內容
    /// </summary>
    public override string FieldValue
    {
        get
        {
            //回傳字串
            //取得表單欄位填寫的內容 
            dsCOMPANY = SERACHtb_COMPANY(TextBox1.Text.Trim());
         
            //<FieldValue MA001='' MA002=''>
            XElement xe = new XElement("FieldValue"
                , new XAttribute("MA001", TextBox1.Text.Trim())
                , new XAttribute("MA002", LabelNAME.Text.Trim())
                , new XAttribute("MA003", LabelNAME.Text.Trim())       
                , new XAttribute("MA004", TextBox13.Text.Trim())
                , new XAttribute("MA005", TextBox14.Text.Trim())
                , new XAttribute("MA006", dsCOMPANY.Tables[0].Rows[0]["PHONE"].ToString().Trim())
                , new XAttribute("MA008", dsCOMPANY.Tables[0].Rows[0]["FAX"].ToString().Trim())
                , new XAttribute("MA009", dsCOMPANY.Tables[0].Rows[0]["EMAIL"].ToString().Trim())
                , new XAttribute("MA010", dsCOMPANY.Tables[0].Rows[0]["TAX_NUMBER"].ToString().Trim())
                , new XAttribute("MA011", TextBox2.Text.Trim())
                , new XAttribute("MA012", dsCOMPANY.Tables[0].Rows[0]["TURNOVER"].ToString().Trim())
                , new XAttribute("MA013", dsCOMPANY.Tables[0].Rows[0]["WORKER_NUMBER"].ToString().Trim())
                , new XAttribute("MA014", DropDownList1.SelectedValue.ToString().Trim())
                , new XAttribute("MA015", DropDownList2.SelectedValue.ToString().Trim())
                , new XAttribute("MA016", TextBox15.Text.Trim())
                , new XAttribute("MA017", DropDownList3.SelectedValue.ToString().Trim())
                , new XAttribute("MA018", DropDownList4.SelectedValue.ToString().Trim())
                , new XAttribute("MA019", DropDownList5.SelectedValue.ToString().Trim())
                , new XAttribute("MA023", dsCOMPANY.Tables[0].Rows[0]["CITY"].ToString().Trim() + dsCOMPANY.Tables[0].Rows[0]["TOWN"].ToString().Trim()+ dsCOMPANY.Tables[0].Rows[0]["ADDRESS"].ToString().Trim())
                , new XAttribute("MA025", dsCOMPANY.Tables[0].Rows[0]["CITY"].ToString().Trim() + dsCOMPANY.Tables[0].Rows[0]["TOWN"].ToString().Trim() + dsCOMPANY.Tables[0].Rows[0]["ADDRESS"].ToString().Trim())
                , new XAttribute("MA027", dsCOMPANY.Tables[0].Rows[0]["CITY"].ToString().Trim() + dsCOMPANY.Tables[0].Rows[0]["TOWN"].ToString().Trim() + dsCOMPANY.Tables[0].Rows[0]["ADDRESS"].ToString().Trim())
                , new XAttribute("MA028", TextBox3.Text.Trim())
                , new XAttribute("MA031", DropDownList6.SelectedItem.ToString().Trim())
                , new XAttribute("MA032", "N")
                , new XAttribute("MA033", TextBox4.Text.Trim())
                , new XAttribute("MA034", "0")
                , new XAttribute("MA035", "2")
                , new XAttribute("MA036", "1")
                , new XAttribute("MA037", DropDownList7.SelectedValue.ToString().Trim())
                , new XAttribute("MA038", DropDownList8.SelectedValue.ToString().Trim())
                , new XAttribute("MA039", "1")
                , new XAttribute("MA040", TextBox5.Text.Trim())
                , new XAttribute("MA041", DropDownList9.SelectedValue.ToString().Trim())
                , new XAttribute("MA042", DropDownList10.SelectedValue.ToString().Trim())
                , new XAttribute("MA043", TextBox6.Text.Trim())
                , new XAttribute("MA044", "0")
                , new XAttribute("MA045", "0")
                , new XAttribute("MA046", TextBox7.Text.Trim())
                , new XAttribute("MA047", "77305")
                , new XAttribute("MA048", DropDownList11.SelectedValue.ToString().Trim())
                , new XAttribute("MA049", TextBox8.Text.Trim())
                , new XAttribute("MA059", "0")
                , new XAttribute("MA060", "0")
                , new XAttribute("MA061", "0")
                , new XAttribute("MA066", "N")
                , new XAttribute("MA071", TextBox9.Text.Trim())
                , new XAttribute("MA075", TextBox10.Text.Trim())
                , new XAttribute("MA076", DropDownList12.SelectedValue.ToString().Trim())
                , new XAttribute("MA083", DropDownList6.SelectedValue.ToString().Trim())
                , new XAttribute("MA084", "N")
                , new XAttribute("MA085", TextBox15.Text.Trim())
                , new XAttribute("MA086", DropDownList13.SelectedValue.ToString().Trim())
                , new XAttribute("MA087", DropDownList14.SelectedValue.ToString().Trim())
                , new XAttribute("MA088", "1")
                , new XAttribute("MA089", "1")
                , new XAttribute("MA090", "1")
                , new XAttribute("MA091", "1")
                , new XAttribute("MA092", "1")
                , new XAttribute("MA093", "1")
                , new XAttribute("MA094", "0")
                , new XAttribute("MA095", "0")
                , new XAttribute("MA096", "N")
                , new XAttribute("MA097", "1")
                , new XAttribute("MA098", TextBox16.Text.Trim())
                , new XAttribute("MA099", TextBox11.Text.Trim())
                , new XAttribute("MA100", DropDownList15.SelectedValue.ToString().Trim())
                , new XAttribute("MA101", "1")
                , new XAttribute("MA102", "1")
                , new XAttribute("MA103", "2")
                , new XAttribute("MA132", "1")
                , new XAttribute("MA133", "1")
                , new XAttribute("MA127", "0")
                , new XAttribute("MA147", "N")
                , new XAttribute("MA148", "1")
                , new XAttribute("MA149", "1")
                , new XAttribute("MA150", "N")
                , new XAttribute("MA162", "N")
                , new XAttribute("UDF02", TextBox12.Text.Trim())
                );

            return xe.ToString();
        }
        set
        {
			//這個屬性不用修改
            base.m_fieldValue = value;
        }
    }


  
    /// <summary>
    /// 是否為第一次填寫
    /// </summary>
    public override bool IsFirstTimeWrite
    {
        get
        {
            //這裡請自行判斷是否為第一次填寫
            //若實作產品標準的控制修改權限必需實作
            //實作此屬性填寫者可修改也才會生效
            //一般是用 m_versionField.Filler == null(沒有記錄填寫者代表沒填過)
            //      和this.FieldValue (當前的值是否為預設的空白) 作比對
            return false;
        }
        set
        {
            //這個屬性不用修改
            base.IsFirstTimeWrite = value;
        }
    }

    /// <summary>
    /// 設定元件狀態
    /// </summary>
    /// <param name="Enabled">是否啟用輸入元件</param>
    public void EnabledControl(bool Enabled)
    {
        Button1.Visible = Enabled;
    }

    /// <summary>
    /// 顯示時欄位初始值
    /// </summary>
    /// <param name="versionField">欄位集合</param>
    public override void SetField(Ede.Uof.WKF.Design.VersionField versionField)
    {
        FieldOptional fieldOptional = versionField as FieldOptional;

        if (fieldOptional != null)
        {
            //若有擴充屬性，可以用該屬性存取
            // fieldOptional.ExtensionSetting

            //TextBox1.Text= fieldOptional.FieldValue;
            //LabelNAME.Text = COMPANY_NAME;

            if(!string.IsNullOrEmpty(fieldOptional.FieldValue))
            {
                XElement xe = XElement.Parse(fieldOptional.FieldValue);

                TextBox1.Text = xe.Attribute("MA001").Value;
                LabelNAME.Text = xe.Attribute("MA002").Value;
                TextBox2.Text = xe.Attribute("MA011").Value;
                TextBox13.Text = xe.Attribute("MA004").Value;
                TextBox14.Text = xe.Attribute("MA005").Value;
                DropDownList1.SelectedValue = xe.Attribute("MA014").Value;
                DropDownList2.SelectedValue = xe.Attribute("MA015").Value;
                TextBox15.Text = xe.Attribute("MA016").Value;
                DropDownList3.SelectedValue = xe.Attribute("MA017").Value;
                DropDownList4.SelectedValue = xe.Attribute("MA018").Value;
                DropDownList5.SelectedValue = xe.Attribute("MA019").Value;
                TextBox3.Text = xe.Attribute("MA028").Value;
                DropDownList6.SelectedValue = xe.Attribute("MA083").Value;
                TextBox4.Text = xe.Attribute("MA033").Value;
                DropDownList7.SelectedValue = xe.Attribute("MA037").Value;
                DropDownList8.SelectedValue = xe.Attribute("MA038").Value;
                TextBox5.Text = xe.Attribute("MA040").Value;
                DropDownList9.SelectedValue = xe.Attribute("MA041").Value;
                DropDownList10.SelectedValue = xe.Attribute("MA042").Value;
                TextBox6.Text = xe.Attribute("MA043").Value;
                TextBox7.Text = xe.Attribute("MA046").Value;
                DropDownList11.SelectedValue = xe.Attribute("MA048").Value;
                TextBox8.Text = xe.Attribute("MA049").Value;
                TextBox9.Text = xe.Attribute("MA071").Value;
                TextBox10.Text = xe.Attribute("MA075").Value;
                DropDownList12.SelectedValue = xe.Attribute("MA076").Value;
                Label12.Text= xe.Attribute("MA031").Value;
                DropDownList13.SelectedValue = xe.Attribute("MA086").Value;
                DropDownList14.SelectedValue = xe.Attribute("MA087").Value;
                TextBox16.Text = xe.Attribute("MA098").Value;
                TextBox11.Text = xe.Attribute("MA099").Value;
                DropDownList15.SelectedValue = xe.Attribute("MA100").Value;
                TextBox12.Text = xe.Attribute("UDF02").Value;
            }

            switch (fieldOptional.FieldMode)
            {
                case FieldMode.Print:
                case FieldMode.View:
                    //觀看和列印都需作沒有權限的處理
                    EnabledControl(false);
                    break;

            }
            
            #region ==============屬性說明==============『』
			//fieldOptional.IsRequiredField『是否為必填欄位,如果是必填(True),如果不是必填(False)』
			//fieldOptional.DisplayOnly『是否為純顯示,如果是(True),如果不是(False),一般在觀看表單及列印表單時,屬性為True』
			//fieldOptional.HasAuthority『是否有填寫權限,如果有填寫權限(True),如果沒有填寫權限(False)』
			//fieldOptional.FieldValue『如果已有人填寫過欄位,則此屬性為記錄其內容』
			//fieldOptional.FieldDefault『如果欄位有預設值,則此屬性為記錄其內容』
			//fieldOptional.FieldModify『是否允許修改,如果允許(fieldOptional.FieldModify=FieldModifyType.yes),如果不允許(fieldOptional.FieldModify=FieldModifyType.no)』
			//fieldOptional.Modifier『如果欄位有被修改過,則Modifier的內容為EBUser,如果沒有被修改過,則會等於Null』
            #endregion

            #region ==============如果有修改，要顯示修改者資訊==============
            if (fieldOptional.Modifier != null)
            {
                lblModifier.Visible = true;
                lblModifier.ForeColor = System.Drawing.Color.FromArgb(0x52, 0x52, 0x52);
                lblModifier.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(fieldOptional.Modifier.Name, true);
            } 
            #endregion
        }
    }

    #region
    protected void btn_Click(object sender, EventArgs e)
    {
        MA001 = TextBox1.Text;
        dsCOMPANY = SERACHtb_COMPANY(MA001);

        LabelNAME.Text = dsCOMPANY.Tables[0].Rows[0]["COMPANY_NAME"].ToString();
    }

    public DataSet SERACHtb_COMPANY(string COMPANY_ID)
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        StringBuilder SQLQUERY = new StringBuilder();

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SQLQUERY.Clear();
            SQLQUERY.AppendFormat(@" SELECT [COMPANY_ID],[COMPANY_NAME],[ERPNO],[TAX_NUMBER],[PHONE],[FAX],[COUNTRY],[CITY],[TOWN],[ADDRESS]");
            SQLQUERY.AppendFormat(@" ,[OVERSEAS_ADDR],[EMAIL],[WEBSITE],[FACEBOOK],[INDUSTRY],[TURNOVER],[WORKER_NUMBER],[EST_DATE],[PARENT_ID],[UPDATE_DATETIME]");
            SQLQUERY.AppendFormat(@" ,[CREATE_DATETIME],[CREATE_USER_ID],[UPDATE_USER_ID],[NOTE],[OWNER_ID],[LAST_CONTACT_DATE],[STATUS]");
            SQLQUERY.AppendFormat(@" FROM [HJ_BM_DB].[dbo].[tb_COMPANY]");
            SQLQUERY.AppendFormat(@" WHERE [ERPNO]=@ERPNO");
            SQLQUERY.AppendFormat(@" ");

            SqlCommand command = new SqlCommand(SQLQUERY.ToString(), conn);
            command.Parameters.AddWithValue("@ERPNO", COMPANY_ID);

            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if(ds.Tables[0].Rows.Count>0)
            {
                return ds;
                //return ds.Tables[0].Rows[0]["COMPANY_NAME"].ToString();  
            }
            else
            {
                return null;
            }
        }
    }

    private void BindDropDownList()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();     
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("MF001", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(@"SELECT LTRIM(RTRIM(MF001)) AS MF001 FROM [TK].dbo.CMSMF ORDER BY MF001", conn);

            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList1.DataSource = ds.Tables[0];
                DropDownList1.DataTextField = "MF001";
                DropDownList1.DataValueField = "MF001";
                DropDownList1.DataBind();

            }
            else
            {

            }
        }


    }

    private void BindDropDownList2()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("ME001", typeof(String));
        dt.Columns.Add("ME002", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(@"SELECT LTRIM(RTRIM(ME001)) AS ME001,ME002 FROM [TK].dbo.CMSME WHERE ME002 NOT LIKE '%停用%' AND (ME001 LIKE '106%' OR ME001 LIKE '116%') ORDER BY ME001 ", conn);

            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList2.DataSource = ds.Tables[0];
                DropDownList2.DataTextField = "ME002";
                DropDownList2.DataValueField = "ME001";
                DropDownList2.DataBind();

            }
            else
            {

            }
        }
    }

    private void BindDropDownList3()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("MR002", typeof(String));
        dt.Columns.Add("MR003", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(@"SELECT LTRIM(RTRIM(MR002)) AS MR002,MR003 FROM [TK].dbo.CMSMR WHERE MR001='1' ORDER BY MR002 ", conn);

            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList3.DataSource = ds.Tables[0];
                DropDownList3.DataTextField = "MR003";
                DropDownList3.DataValueField = "MR002";
                DropDownList3.DataBind();

            }
            else
            {

            }
        }
    }

    private void BindDropDownList4()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("MR002", typeof(String));
        dt.Columns.Add("MR003", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(@"SELECT LTRIM(RTRIM(MR002)) AS MR002,MR003 FROM [TK].dbo.CMSMR WHERE MR001='3' ORDER BY MR002", conn);

            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList4.DataSource = ds.Tables[0];
                DropDownList4.DataTextField = "MR003";
                DropDownList4.DataValueField = "MR002";
                DropDownList4.DataBind();

            }
            else
            {

            }
        }
    }
    private void BindDropDownList5()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("MR002", typeof(String));
        dt.Columns.Add("MR003", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(@"SELECT LTRIM(RTRIM(MR002)) AS MR002,MR003 FROM [TK].dbo.CMSMR WHERE MR001='4' ORDER BY MR002", conn);

            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList5.DataSource = ds.Tables[0];
                DropDownList5.DataTextField = "MR003";
                DropDownList5.DataValueField = "MR002";
                DropDownList5.DataBind();

            }
            else
            {

            }
        }
    }
    private void BindDropDownList6()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("NA002", typeof(String));
        dt.Columns.Add("NA003", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(@"SELECT LTRIM(RTRIM(NA002)) AS NA002,NA003 FROM [TK].dbo.CMSNA WHERE COMPANY='TK' AND NA001='2' ORDER BY NA002", conn);

            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList6.DataSource = ds.Tables[0];
                DropDownList6.DataTextField = "NA003";
                DropDownList6.DataValueField = "NA002";
                DropDownList6.DataBind();

            }
            else
            {

            }
        }
    }

    private void BindDropDownList7()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("Filed1", typeof(String));
        dt.Columns.Add("Filed2", typeof(String));
        //1.二聯式、2.三聯式、3.二聯式收銀機發票、4.三聯式收銀機發票、5.電子計算機發票、6.免用統一發票、7.電子發票

        dt.Rows.Add(new Object[] {"7", "7電子發票" });
        dt.Rows.Add(new Object[] {"1", "1二聯式" });
        dt.Rows.Add(new Object[] { "2", "2三聯式" });
        dt.Rows.Add(new Object[] { "3", "3二聯式收銀機發票" });
        dt.Rows.Add(new Object[] { "4", "4三聯式收銀機發票" });
        dt.Rows.Add(new Object[] { "5", "5電子計算機發票" });
        dt.Rows.Add(new Object[] { "6", "6免用統一發票" });



        DropDownList7.DataSource = dt;
        DropDownList7.DataTextField = "Filed2";
        DropDownList7.DataValueField = "Filed1";
        DropDownList7.DataBind();
    }

    private void BindDropDownList8()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("Filed1", typeof(String));
        dt.Columns.Add("Filed2", typeof(String));
        //1.應稅內含、2.應稅外加、3.零稅率、4.免稅、9.不計稅

        dt.Rows.Add(new Object[] { "1", "1應稅內含" });
        dt.Rows.Add(new Object[] { "2", "2應稅外加" });
        dt.Rows.Add(new Object[] { "3", "3零稅率" });
        dt.Rows.Add(new Object[] { "4", "4免稅" });
        dt.Rows.Add(new Object[] { "5", "5不計稅" });


        DropDownList8.DataSource = dt;
        DropDownList8.DataTextField = "Filed2";
        DropDownList8.DataValueField = "Filed1";
        DropDownList8.DataBind();
    }

    private void BindDropDownList9()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("Filed1", typeof(String));
        dt.Columns.Add("Filed2", typeof(String));
        //1.現金、2.電匯、3.支票、4.其他

        dt.Rows.Add(new Object[] { "1", "1現金" });
        dt.Rows.Add(new Object[] { "2", "2電匯" });
        dt.Rows.Add(new Object[] { "3", "3支票" });
        dt.Rows.Add(new Object[] { "4", "4其他" });



        DropDownList9.DataSource = dt;
        DropDownList9.DataTextField = "Filed2";
        DropDownList9.DataValueField = "Filed1";
        DropDownList9.DataBind();
    }

    private void BindDropDownList10()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("Filed1", typeof(String));
        dt.Columns.Add("Filed2", typeof(String));
        //1:郵寄、2.自領、3.其它

        dt.Rows.Add(new Object[] { "1", "1郵寄" });
        dt.Rows.Add(new Object[] { "2", "2自領" });
        dt.Rows.Add(new Object[] { "3", "3其它" });


        DropDownList10.DataSource = dt;
        DropDownList10.DataTextField = "Filed2";
        DropDownList10.DataValueField = "Filed1";
        DropDownList10.DataBind();
    }
    private void BindDropDownList11()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("Filed1", typeof(String));
        dt.Columns.Add("Filed2", typeof(String));
        //1.空運、2.海運、3.海空聯運、4.郵寄、5.陸運、6.自取、7.自送、8.快遞  

        dt.Rows.Add(new Object[] { "1", "1空運" });
        dt.Rows.Add(new Object[] { "2", "2海運" });
        dt.Rows.Add(new Object[] { "3", "3海空聯運" });
        dt.Rows.Add(new Object[] { "4", "4郵寄" });
        dt.Rows.Add(new Object[] { "5", "5陸運" });
        dt.Rows.Add(new Object[] { "6", "6自取" });
        dt.Rows.Add(new Object[] { "7", "7自送" });
        dt.Rows.Add(new Object[] { "8", "8快遞" });

        DropDownList11.DataSource = dt;
        DropDownList11.DataTextField = "Filed2";
        DropDownList11.DataValueField = "Filed1";
        DropDownList11.DataBind();
    }

    private void BindDropDownList12()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("MR002", typeof(String));
        dt.Columns.Add("MR003", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(@"SELECT LTRIM(RTRIM(MR002)) AS MR002,MR003 FROM [TK].dbo.CMSMR WHERE MR001='4' ORDER BY MR002", conn);

            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList12.DataSource = ds.Tables[0];
                DropDownList12.DataTextField = "MR003";
                DropDownList12.DataValueField = "MR002";
                DropDownList12.DataBind();

            }
            else
            {

            }
        }
    }

    private void BindDropDownList13()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("Filed1", typeof(String));
        dt.Columns.Add("Filed2", typeof(String));
        //Y,N  

        dt.Rows.Add(new Object[] { "Y", "Y" });
        dt.Rows.Add(new Object[] { "N", "N" });

        DropDownList13.DataSource = dt;
        DropDownList13.DataTextField = "Filed2";
        DropDownList13.DataValueField = "Filed1";
        DropDownList13.DataBind();
    }

    private void BindDropDownList14()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("Filed1", typeof(String));
        dt.Columns.Add("Filed2", typeof(String));
        //1.整張資料計算 2.單身單筆資料計算  

        dt.Rows.Add(new Object[] { "1", "1整張資料計算" });
        dt.Rows.Add(new Object[] { "2", "2單身單筆資料計算" });
    
        DropDownList14.DataSource = dt;
        DropDownList14.DataTextField = "Filed2";
        DropDownList14.DataValueField = "Filed1";
        DropDownList14.DataBind();
    }
    private void BindDropDownList15()
    {
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();
        DataRow ndr = dt.NewRow();

        dt.Columns.Add("Filed1", typeof(String));
        dt.Columns.Add("Filed2", typeof(String));
        //1.自然人 2.法人

        dt.Rows.Add(new Object[] { "1", "1自然人" });
        dt.Rows.Add(new Object[] { "2", "2法人" });

        DropDownList15.DataSource = dt;
        DropDownList15.DataTextField = "Filed2";
        DropDownList15.DataValueField = "Filed1";
        DropDownList15.DataBind();
    }

    #endregion

}