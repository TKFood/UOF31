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
using System.Text;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO;
using System.Net;

public partial class WKF_OptionalFields_optionField_INFORM : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
{
    string FormId = "";
    string FormNumber = "";
    string TaskId = "";
    string ApplicantGuid = "";
    string APPLICANT_NAME = "";
    string APPLICANT_ACCOUNT = "";
    string CURRENTNAME = "";
    string CURRENTACCOUNT = "";
    string CURRENTTITLENAME = "";
    string CURRENTRANK = "";
    string APPLICANT_EMAIL = "";
    string FORM_NAME = "";
    string COMMENT = "";

    int CHECK_CURRENTRANK = 999;

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

    protected void Page_Load(object sender, EventArgs e)
    {
        //找出表單申請人員
        //找出目前的簽核人員
        try
        {

            FormId = base.taskObj.FormId;
            FormNumber = base.taskObj.FormNumber;
            TaskId = base.taskObj.TaskId;
            ApplicantGuid = base.ApplicantGuid;

            if (!string.IsNullOrEmpty(FormNumber))
            {
                DataTable DT = SEARCHFORMCURRENT(FormNumber);

                if (DT.Rows.Count >= 1 && DT != null)
                {
                    APPLICANT_NAME = DT.Rows[0]["APPLICANT_NAME"].ToString();
                    APPLICANT_ACCOUNT = DT.Rows[0]["APPLICANT_ACCOUNT"].ToString();
                    CURRENTNAME = DT.Rows[0]["CURRENTNAME"].ToString();
                    CURRENTACCOUNT = DT.Rows[0]["CURRENTACCOUNT"].ToString();
                    CURRENTTITLENAME = DT.Rows[0]["CURRENTTITLENAME"].ToString();
                    APPLICANT_EMAIL = DT.Rows[0]["APPLICANT_EMAIL"].ToString();
                    FORM_NAME = DT.Rows[0]["FORM_NAME"].ToString();
                    CURRENTRANK = DT.Rows[0]["CURRENTRANK"].ToString();
                    COMMENT= DT.Rows[0]["COMMENT"].ToString();

                    CHECK_CURRENTRANK = Convert.ToInt32(CURRENTRANK);
                }
            }

            //if (CHECK_CURRENTRANK <= 11)
            //{
            //    Button1.Visible = true;
            //}
            //else
            //{
            //    Button1.Visible = false;

            //}
        }

        catch
        {

        }

        try
        {
            SETDropDownList1();
        }
        catch
        {

        }



        

        //這裡不用修改
        //欄位的初始化資料都到SetField Method去做
        SetField(m_versionField);
        
        //檢查職級，要大於課長，才能按呼叫
       
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
			return String.Empty;
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
            return String.Empty;
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
            return String.Empty;
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
			return String.Empty;
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
			return String.Empty;
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

            
            //草稿
            if(!fieldOptional.IsAudit)
            {
                if(fieldOptional.HasAuthority)
                {
                    //有填寫權限的處理
                    EnabledControl(true);

                    DropDownList1.Visible = false;
                    Button1.Visible = false;
                }
                else
                {
                    //沒填寫權限的處理
                    EnabledControl(false);

                    DropDownList1.Visible = false;
                    Button1.Visible = false;
                }
            }
            else
            {
                //己送出

                //有填過
                if(fieldOptional.Filler != null)
                {
                    //判斷填寫的站點和當前是否相同
                    if(base.taskObj != null && base.taskObj.CurrentSite != null &&
                        base.taskObj.CurrentSite.SiteId == fieldOptional.FillSiteId && fieldOptional.Filler.UserGUID == Ede.Uof.EIP.SystemInfo.Current.UserGUID)
                    {
                        //判斷填寫權限
                        if (fieldOptional.HasAuthority)
                        {
                            //有填寫權限的處理
                            EnabledControl(true);

                            DropDownList1.Visible = true;
                            Button1.Visible = true;
                        }
                        else
                        {
                            //沒填寫權限的處理
                            EnabledControl(false);


                            DropDownList1.Visible = false;
                            Button1.Visible = false;
                        }
                    }
                    else
                    {
                        //判斷修改權限
                        if (fieldOptional.AllowModify)
                        {
                            //有修改權限的處理
                            EnabledControl(true);

                            DropDownList1.Visible = true;
                            Button1.Visible = true;
                        }
                        else
                        {
                            //沒修改權限的處理
                            EnabledControl(false);

                            DropDownList1.Visible = false;
                            Button1.Visible = false;
                        }

                    }
                }
                else
                {
                    //判斷填寫權限
                    if (fieldOptional.HasAuthority)
                    {
                        //有填寫權限的處理
                        EnabledControl(true);

                        DropDownList1.Visible = true;
                        Button1.Visible = true;
                    }
                    else
                    {
                        //沒填寫權限的處理
                        EnabledControl(false);


                        DropDownList1.Visible = false;
                        Button1.Visible = false;
                    }

                }
            }



            switch(fieldOptional.FieldMode)
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

    #region FUNCTION
    public void ADDToUOF_TB_EIP_PRIV_MESS(string MESSAGE_TO, string MESSAGE_FROM, string TOPIC, string CONTENT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();

        Guid MESSAGE_GUID = Guid.NewGuid();
        Guid MASTER_GUID = Guid.NewGuid();
        Guid NOTIFY_ID = Guid.NewGuid();

        StringBuilder SBTEXT = new StringBuilder();

        //MESSAGE_TO = "b6f50a95-17ec-47f2-b842-4ad12512b431";
        //MESSAGE_FROM = "b6f50a95-17ec-47f2-b842-4ad12512b431";
        //TOPIC = "TEST";

        //SBTEXT.AppendFormat(@"
        //                    <p style=""font-size:160%;color:red;"">This is a paragraph.</p>
        //                    <br></br>
        //                    <p style=""font-size:160%;color:blue;"">This is a paragraph.</p>
        //                      ");
        //CONTENT = SBTEXT.ToString();

        string CREATOR = MESSAGE_FROM;
        string MODIFIER = MESSAGE_FROM;
        string MESSAGE_TOUSER = @"<UserSet><Element type=""user""><userId>b6f50a95-17ec-47f2-b842-4ad12512b431</userId></Element></UserSet>";
        string MESSAGE_CONTENT = CONTENT;
        string TB_EIP_PRIV_MESS_CREATE_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + "+08:00";
        string CREATE_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        string SENDER_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        string CREATE_FROM = "192.168.1.57";
        string MODIFY_FROM = "192.168.1.57";
        string CREATE_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        string MODIFY_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        string USER_GUID = MESSAGE_FROM;
        string TITLE = TOPIC;

        try
        {
            StringBuilder queryString = new StringBuilder();
            queryString.AppendFormat(@"

                                    INSERT [dbo].TB_EIP_PRIV_MESS
                                    ( 
                                    MESSAGE_GUID
                                    , TOPIC
                                    , MESSAGE_CONTENT
                                    , MESSAGE_TO
                                    , MESSAGE_FROM
                                    , CREATE_TIME
                                    , FROM_DELETED
                                    , TO_DELETED
                                    , FILE_GROUP_ID
                                    , MASTER_GUID 
                                    ) 
                                    VALUES 
                                    ( 
                                    @MESSAGE_GUID
                                    , @TOPIC
                                    , @MESSAGE_CONTENT
                                    , @MESSAGE_TO
                                    , @MESSAGE_FROM
                                    , @TB_EIP_PRIV_MESS_CREATE_TIME
                                    , 0
                                    , 0
                                    , N''
                                    , @MASTER_GUID
                                    )

                                    INSERT [dbo].TB_EIP_PRIV_MESS_MASTER
                                    ( 
                                    MASTER_GUID
                                    , TOPIC
                                    , MESSAGE_FROM
                                    , MESSAGE_TO
                                    , SENDER_TIME
                                    , CREATOR
                                    , CREATE_FROM
                                    , CREATE_DATE
                                    , MODIFIER
                                    , MODIFY_FROM
                                    , MODIFY_DATE
                                    ) 
                                    VALUES 
                                    (  
                                    @MESSAGE_GUID
                                    ,@TOPIC
                                    ,@MESSAGE_FROM
                                    ,@MESSAGE_TOUSER
                                    ,@SENDER_TIME
                                    ,@CREATOR 
                                    ,@CREATE_FROM
                                    ,@CREATE_DATE
                                    ,@MODIFIER
                                    ,@MODIFY_FROM
                                    ,@MODIFY_DATE
                                    )

                                    INSERT [dbo].TB_EIP_PUSH_QUEUE
                                    ( 
                                    [NOTIFY_ID]
                                    , [USER_GUID]
                                    , [DESCRIPTION]
                                    , [TITLE]
                                    , [DISPLAY_NUMBER]
                                    , [MODULE_NAME]
                                    , [MODULE_KEY]
                                    , [CREATOR]
                                    , [CREATE_FROM]
                                    , [CREATE_DATE]
                                    , [MODIFIER]
                                    , [MODIFY_FROM]
                                    , [MODIFY_DATE]
                                    )
                                    VALUES
                                    (
                                    @NOTIFY_ID
                                    , @USER_GUID
                                    , N''
                                    ,@TITLE
                                    , 1
                                    , N'PrivateMessage'
                                    , N'PrivateMessage?id=@MESSAGE_GUID'
                                    ,@CREATOR
                                    , @CREATE_FROM
                                    , @CREATE_DATE
                                    , @MODIFIER
                                    , @MODIFY_FROM
                                    , @MODIFY_DATE
                                    )

                                        ");

          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString.ToString(), connection);
            
                command.Parameters.AddWithValue("@MESSAGE_GUID", MESSAGE_GUID);
                command.Parameters.AddWithValue("@MASTER_GUID", MASTER_GUID);
                command.Parameters.AddWithValue("@NOTIFY_ID", NOTIFY_ID);
                command.Parameters.AddWithValue("@MESSAGE_FROM", MESSAGE_FROM);
                command.Parameters.AddWithValue("@MESSAGE_TO", MESSAGE_TO);
                command.Parameters.AddWithValue("@MESSAGE_TOUSER", MESSAGE_TOUSER);
                command.Parameters.AddWithValue("@TOPIC", TOPIC);
                command.Parameters.AddWithValue("@MESSAGE_CONTENT", MESSAGE_CONTENT);
                command.Parameters.AddWithValue("@CREATE_TIME", CREATE_TIME);
                command.Parameters.AddWithValue("@SENDER_TIME", SENDER_TIME);
                command.Parameters.AddWithValue("@CREATOR", CREATOR);
                command.Parameters.AddWithValue("@MODIFIER", MODIFIER);
                command.Parameters.AddWithValue("@CREATE_FROM", CREATE_FROM);
                command.Parameters.AddWithValue("@MODIFY_FROM", MODIFY_FROM);
                command.Parameters.AddWithValue("@CREATE_DATE", CREATE_DATE);
                command.Parameters.AddWithValue("@MODIFY_DATE", MODIFY_DATE);
                command.Parameters.AddWithValue("@TITLE", TITLE);
                command.Parameters.AddWithValue("@USER_GUID", USER_GUID);
                command.Parameters.AddWithValue("@TB_EIP_PRIV_MESS_CREATE_TIME", TB_EIP_PRIV_MESS_CREATE_TIME);

                command.Connection.Open();

                int count = command.ExecuteNonQuery();
                //MsgBox("MESSAGE "+ count, this.Page, this);

                connection.Close();
                connection.Dispose();

            }
          

        }
        catch
        {

        }

        finally
        {
          
        }

    }

    private DataTable SEARCHFORMCURRENT(string DOC_NBR)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            SELECT
                            usr2.NAME AS 'CURRENTNAME'
                            , usr2.ACCOUNT AS 'CURRENTACCOUNT'
                            ,[TB_EB_JOB_TITLE].TITLE_NAME AS 'CURRENTTITLENAME'
                            ,[TB_EB_JOB_TITLE].RANK AS 'CURRENTRANK'
                            ,(CASE WHEN  usr.IS_SUSPENDED = 1 THEN  usr.NAME + '(x)' WHEN  ISNULL(usr.ACCOUNT,'''') = '' THEN  'unknown user' ELSE usr.NAME END) AS APPLICANT_NAME
                            ,(CASE WHEN  usr.IS_SUSPENDED = 1 THEN  usr.NAME + '(x)' WHEN  ISNULL(usr.ACCOUNT,'''') = '' THEN  'unknown user' ELSE usr.ACCOUNT END) AS APPLICANT_ACCOUNT
                            ,usr.[EMAIL] AS 'APPLICANT_EMAIL'
                            ,form.FORM_NAME
                            ,DOC_NBR
                            ,CONVERT(NVARCHAR,NODES.START_TIME,111) AS 'START_TIME'
                            ,DATEDIFF(HOUR,START_TIME,GETDATE()) AS 'HRS'
                            ,CONVERT(NVARCHAR,BEGIN_TIME,111) AS BEGIN_TIME
                            ,task.TASK_ID
                            ,END_TIME
                            ,TASK_RESULT
                            ,TASK_STATUS
                            ,task.USER_GUID
                            ,formVer.FORM_VERSION_ID
                            ,formVer.FORM_ID
                            ,CURRENT_SITE_ID
                            ,MESSAGE_CONTENT
                            ,LOCK_STATUS
                            ,ISNULL(formVer.DISPLAY_TITLE,'') AS VERSION_TITLE
                            ,ISNULL(task.JSON_DISPLAY,'') AS JSON_DISPLAY
                            ,[NODES].SIGN_STATUS
                            ,[NODES].COMMENT

                            FROM dbo.TB_WKF_TASK task
                            INNER JOIN dbo.TB_WKF_FORM_VERSION formVer ON task.FORM_VERSION_ID = formVer.FORM_VERSION_ID
                            INNER JOIN dbo.TB_WKF_FORM form  ON  formVer.FORM_ID = form.FORM_ID 
                            LEFT JOIN dbo.TB_EB_USER [usr]  ON task.USER_GUID = usr.USER_GUID
                            LEFT JOIN dbo.TB_WKF_TASK_NODE [NODES] ON NODES.SITE_ID=task.CURRENT_SITE_ID 
                            LEFT JOIN dbo.TB_EB_USER [usr2]  ON NODES.ORIGINAL_SIGNER = [usr2].USER_GUID
                            LEFT JOIN dbo.[TB_EB_EMPL_DEP] ON [TB_EB_EMPL_DEP].USER_GUID=[usr2].USER_GUID
                            LEFT JOIN dbo.[TB_EB_JOB_TITLE] ON [TB_EB_EMPL_DEP].TITLE_ID=[TB_EB_JOB_TITLE].TITLE_ID

                            WHERE
                            1=1  
                           
                            AND ISNULL([NODES].SIGN_STATUS,999)<>0
                            AND DOC_NBR LIKE '%{0}%'
                            ORDER BY HRS DESC,usr2.NAME,form.FORM_NAME,DOC_NBR
                               
                                ", DOC_NBR);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if(dt.Rows.Count>=1)
        {
            return dt;
        }
        else
        {
            return null;
        }
     
    }

    /// <summary>
    /// 完整的寄信函數
    /// </summary>
    /// <param name="MailFrom">寄信人Email Address</param>
    /// <param name="MailTos">收信人Email Address</param>
    /// <param name="Ccs">副本Email Address</param>
    /// <param name="MailSub">主旨</param>
    /// <param name="MailBody">內文</param>
    /// <param name="isBodyHtml">是否為Html格式</param>
    /// <param name="files">要夾帶的附檔</param>
    /// <returns>回傳寄信是否成功(true:成功,false:失敗)</returns>
    public bool Mail_Send(string MailFrom, string[] MailTos, string MailSub, string MailBody, bool isBodyHtml, Dictionary<string, Stream> files)
    {
        /// <summary>
        /// 寄信標題
        /// </summary>
        string mailTitle = "寄信標題";
        /// <summary>
        /// 寄信人Email
        /// </summary>
        MailFrom = "tk290@tkfood.com.tw";
        /// <summary>
        /// 收信人Email(多筆用逗號隔開)
        /// </summary>
        string receiveMails = "";
        /// <summary>
        /// 寄信smtp server
        /// </summary>
        string smtpServer = ConfigurationManager.AppSettings["smtpServer"].Trim();
        /// <summary>
        /// 寄信smtp server的Port，預設25
        /// </summary>
        int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"].Trim());
        /// <summary>
        /// 寄信帳號
        /// </summary>
        string mailAccount = ConfigurationManager.AppSettings["mailAccount"].Trim();
        /// <summary>
        /// 寄信密碼
        /// </summary>
        string mailPwd = ConfigurationManager.AppSettings["mailPwd"].Trim();

        //MsgBox("smtpServer:"+ smtpServer, this.Page, this);
        //MsgBox("smtpPort:" + smtpPort, this.Page, this);
        //MsgBox("mailAccount:" +mailAccount , this.Page, this);
        //MsgBox("mailPwd:" + mailPwd, this.Page, this);
  
        try
        {
            //命名空間： System.Web.Mail已過時，http://msdn.microsoft.com/zh-tw/library/system.web.mail.mailmessage(v=vs.80).aspx
            //建立MailMessage物件
            MailMessage mms = new MailMessage();
            //指定一位寄信人MailAddress
            mms.From = new MailAddress(MailFrom);
            //信件主旨
            mms.Subject = MailSub;
            //信件內容
            mms.Body = MailBody;
            //信件內容 是否採用Html格式
            mms.IsBodyHtml = isBodyHtml;

            if (MailTos != null)//防呆
            {
                for (int i = 0; i < MailTos.Length; i++)
                {
                    //加入信件的收信人(們)address
                    if (!string.IsNullOrEmpty(MailTos[i].Trim()))
                    {
                        mms.Bcc.Add(new MailAddress(MailTos[i].Trim()));
                    }

                }
            }//End if (MailTos !=null)//防呆
           

            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))//或公司、客戶的smtp_server
            {
                if (!string.IsNullOrEmpty(mailAccount) && !string.IsNullOrEmpty(mailPwd))//.config有帳密的話
                {
                    client.Credentials = new NetworkCredential(mailAccount, mailPwd);//寄信帳密
                }
                client.Send(mms);//寄出一封信
            }

            //MsgBox("MAILS true", this.Page, this);
            return true;//成功
        }
        catch (Exception ex)
        {
            //MsgBox("MAILS false " + ex.ToString() , this.Page, this);
            return false;
        }
    }//End 寄信

    public void SENDMESSAGE(string ApplicantGuid,string SUBJECTMESSAGES,string CONTEXTMESSAGES)
    {
        ADDToUOF_TB_EIP_PRIV_MESS(ApplicantGuid, ApplicantGuid, SUBJECTMESSAGES, CONTEXTMESSAGES);

    }

    public void SENDEMAIL(string[] MAILTO,string SUBJECTMESSAGES,string CONTEXTMESSAGES)
    {      
        this.Mail_Send("", MAILTO, SUBJECTMESSAGES, CONTEXTMESSAGES, true, null);
    }
    public void MsgBox(String ex, Page pg, Object obj)
    {

        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }
    public void SETDropDownList1()
    {
        DataTable dt = SEARCH_MANAGER();

        DropDownList1.DataSource = dt;
        DropDownList1.DataTextField = "NAME";
        DropDownList1.DataValueField = "ACCOUNT";
        DropDownList1.DataBind();
      

        if (!string.IsNullOrEmpty(APPLICANT_NAME))
        {
            DropDownList1.Items.Add("表單申請人:"+APPLICANT_NAME);
        }


    }
    private DataTable SEARCH_MANAGER()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        cmdTxt.AppendFormat(@"
                            SELECT 
                            [ID]
                            ,[Z_UOF_optionField_INFORM].[ACCOUNT]
                            ,[Z_UOF_optionField_INFORM].[NAME]
                            ,[ISUSED]
                            ,[USER_GUID]
                            ,[EMAIL]
                            FROM [UOF].[dbo].[Z_UOF_optionField_INFORM]
                            LEFT JOIN [dbo].[TB_EB_USER] ON [Z_UOF_optionField_INFORM].[ACCOUNT]=[TB_EB_USER].[ACCOUNT]
                            ORDER BY [ID]                         
                               
                                ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count >= 1)
        {
            return dt;
        }
        else
        {
            return null;
        }

    }

    private DataTable SEARCH_UOF_USERS(string ACCOUNT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        cmdTxt.AppendFormat(@" 

                            SELECT 
                            [USER_GUID]
                            ,[ACCOUNT]
                            ,[NAME]
                            ,[EMAIL]
                            ,[PASSWORD]
                            ,[IS_PASSWORD_RESET]
                            ,[CREATE_DATE]
                            ,[IS_LOCKED_OUT]
                            ,[LAST_ACTIVITY_DATE]
                            ,[LAST_LOCKED_OUT_DATE]
                            ,[LAST_LOGIN_DATE]
                            ,[LAST_PASSWORD_CHANGE_DATE]
                            ,[USER_TYPE]
                            ,[EXPIRE_DATE]
                            ,[THEME]
                            ,[MSG_TYPE]
                            ,[IS_AD_AUTH]
                            ,[EMAIL_A]
                            ,[EMAIL_B]
                            ,[EMAIL_C]
                            ,[EMAIL_D]
                            ,[PASSWORD_INVALID_ATTEMPTS]
                            ,[PW_RESET_REASON]
                            ,[IS_USB_AUTH]
                            ,[USB_KEY]
                            ,[IS_SUSPENDED]
                            ,[LAST_SUSPENDED_DATE]
                            ,[SID]
                            ,[OPTION1]
                            ,[OPTION2]
                            ,[OPTION3]
                            ,[OPTION4]
                            ,[OPTION5]
                            ,[OPTION6]
                            ,[PERSONAL1]
                            ,[PERSONAL2]
                            ,[PERSONAL3]
                            ,[PERSONAL4]
                            ,[PERSONAL5]
                            ,[PERSONAL6]
                            ,[NICKNAME]
                            ,[CA_SERIAL_NUM]
                            ,[PROXY]
                            ,[DOMAIN]
                            ,[ACCOUNT_MAPPING]
                            ,[IS_UPDATE_PERSONAL_INFO]
                            ,[LAST_UPDATE_PERSONAL_INFO_DATE]
                            ,[TIMEZONE_TEXT]
                            ,[LOCATION_ID]
                            ,[DISPLAY_TIMEZONE]
                            ,[COMPANY_UNIFIED_ID]
                            ,[COMPANY_NO]
                            ,[AFTER_TRANS_MAIL_MODE]
                            ,[SCHEME_ID]
                            FROM [UOF].[dbo].[TB_EB_USER]
                            WHERE EMAIL LIKE '%tkfood%'
                            AND [ACCOUNT]='{0}'

                          
                               
                                ", ACCOUNT);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count >= 1)
        {
            return dt;
        }
        else
        {
            return null;
        }

    }

    #endregion

    #region BUTTON
    protected void Button1_Click(object sender, EventArgs e)
    {
        string NAME = DropDownList1.SelectedItem.Text.ToString();
        string ACCOUNT = DropDownList1.SelectedValue.ToString();

        if (NAME.Contains("表單申請人"))
        {
            ACCOUNT = APPLICANT_ACCOUNT;
        }

        if(!string.IsNullOrEmpty(ACCOUNT))
        {
            DataTable DT = SEARCH_UOF_USERS(ACCOUNT);
            try
            {
                if (DT != null)
                {
                    string USER_GUID = DT.Rows[0]["USER_GUID"].ToString();
                    string EMAIL = DT.Rows[0]["EMAIL"].ToString();

                    string SUBJECTMESSAGES = "通知人員 [ " + NAME + " ] 您好" + "  ，[" + CURRENTNAME + "][" + CURRENTTITLENAME + "]" + " 針對表單: [" + FORM_NAME + "] 單號: [" + FormNumber + "] " + "有問題需要進一步了解，" + "敬請您跟 [" + CURRENTNAME + "][" + CURRENTTITLENAME + "] 說明，謝謝。";
                    string CONTEXTMESSAGES = "通知人員 [ " + NAME + " ] 您好" + "  ，[" + CURRENTNAME + "][" + CURRENTTITLENAME + "]" + " 針對表單: [" + FORM_NAME + "] 單號: [" + FormNumber + "] " + "有問題需要進一步了解，" + "敬請您跟 [" + CURRENTNAME + "][" + CURRENTTITLENAME + "] 說明，謝謝。";
                    //string SUBJECTMESSAGES = "[" + CURRENTNAME + "]  [" + CURRENTTITLENAME + "] ，通知 [" + NAME + "] " + " 表單: [" + FORM_NAME + "] 單號: [" + FormNumber + "] " + " ，請跟 [" + CURRENTNAME + "] [" + CURRENTTITLENAME + "] 說明表單內容，謝謝。";
                    //string CONTEXTMESSAGES = "[" + CURRENTNAME + "]  [" + CURRENTTITLENAME + "] ，通知 [" + NAME + "] " + " 表單: [" + FORM_NAME + "] 單號: [" + FormNumber + "] " + " ，請跟 [" + CURRENTNAME + "] [" + CURRENTTITLENAME + "] 說明表單內容，謝謝。";

                    SENDMESSAGE(USER_GUID, SUBJECTMESSAGES, CONTEXTMESSAGES);
                    SENDEMAIL(new string[] { EMAIL }, SUBJECTMESSAGES, CONTEXTMESSAGES);

                    MsgBox(FormNumber + " 已通知:" + NAME + " " + ACCOUNT, this.Page, this);
                }
            }
            catch
            {

            }
           
          
        }

        //if (!string.IsNullOrEmpty(FormNumber))
        //{
        //    DataTable DT = SEARCHFORMCURRENT(FormNumber);

        //    if (DT.Rows.Count >= 1 && DT != null)
        //    {
        //        APPLICANT_NAME = DT.Rows[0]["APPLICANT_NAME"].ToString();
        //        CURRENTNAME = DT.Rows[0]["CURRENTNAME"].ToString();
        //        CURRENTTITLENAME = DT.Rows[0]["CURRENTTITLENAME"].ToString();
        //        APPLICANT_EMAIL = DT.Rows[0]["APPLICANT_EMAIL"].ToString();
        //        FORM_NAME = DT.Rows[0]["FORM_NAME"].ToString();

        //        string SUBJECTMESSAGES = "[" + CURRENTNAME + "] [" + CURRENTTITLENAME + "] ，呼叫表單申請人 [" + APPLICANT_NAME + "] " + " 表單: [" + FORM_NAME + "] 單號: [" + FormNumber + "] " + " ，請跟 [" + CURRENTNAME + "] [" + CURRENTTITLENAME + "] 說明表單內容，謝謝。";
        //        string CONTEXTMESSAGES = "[" + CURRENTNAME + "] [" + CURRENTTITLENAME + "] ，呼叫表單申請人 [" + APPLICANT_NAME + "] " + " 表單: [" + FORM_NAME + "] 單號: [" + FormNumber + "] " + " ，請跟 [" + CURRENTNAME + "] [" + CURRENTTITLENAME + "] 說明表單內容，謝謝。";


        //        SENDMESSAGE(ApplicantGuid, SUBJECTMESSAGES, CONTEXTMESSAGES);
        //        SENDEMAIL(new string[] { APPLICANT_EMAIL }, SUBJECTMESSAGES, CONTEXTMESSAGES);
        //    }




        //    MsgBox(FormNumber+" 已通知申請人", this.Page, this);

        //}






    }
    #endregion

   
}