using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Log;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

public partial class CDS_WebPage_COWORK_DY_PURTE_PURTF_FORM : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    string TC001 = "";
    string TC002 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            BindDropDownList();

            SETTEXT();
        }

    }

    #region FUNCTION
    public void SETTEXT()
    {
        DateTime DT = DateTime.Now;
        string NOWYEARS = DT.Year.ToString();
        string NOWMONTHS = DT.Month.ToString();

        TextBox1.Text = NOWYEARS;
        TextBox2.Text = NOWMONTHS;
    }
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "STATUS";
            DropDownList1.DataValueField = "STATUS";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();


        //日期
        if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox2.Text))
        {
            if (TextBox2.Text.Length == 1)
            {
                TextBox2.Text = "0" + TextBox2.Text;
            }
            QUERYS.AppendFormat(@" AND TE002 LIKE '{0}%'", TextBox1.Text.Trim() + TextBox2.Text.Trim());

        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TE017='N'");
            }
            else if (DropDownList1.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TE017='Y'");
            }
        }

        cmdTxt.AppendFormat(@" 
                                SELECT 
                                REPLACE(TE001+TE002+TE003,' ','') AS TE001TE002TE003,
                                TE001,
                                TE002,
                                TE003,
                                TE005,
                                MA002,
                                STUFF((
                                        SELECT ',' + TF006+' ,數量'+CONVERT(NVARCHAR,TF009)+' ,到貨日'+TF013
                                        FROM [DY].dbo.PURTF
                                        WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                        FOR XML PATH(''), TYPE
                                    ).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS DETAILS

                                FROM [DY].dbo.PURTE,[DY].dbo.PURMA
                                WHERE TE005=MA001
                                {0}
                                {1}
                                ORDER BY TE001,PURTE.TE002,PURTE.TE003
                                ", QUERYS.ToString(), QUERYS2.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {         

            //Button2
            //Get the button that raised the event
            Button btn2 = (Button)e.Row.FindControl("Button2");
            //Get the row that contains this button
            GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue2 = btn2.CommandArgument;
            DataRowView row2 = (DataRowView)e.Row.DataItem;
            Button lbtnName2 = (Button)e.Row.FindControl("Button2");
            ExpandoObject param2 = new { ID = Cellvalue2 }.ToExpando();


        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

         if (e.CommandName == "Button2")
        {
            //檢查是否已簽核 或是 在簽核中
            DataTable DT = CHECK_TB_WKF_TASK(e.CommandArgument.ToString());

            if (DT != null && DT.Rows.Count >= 1)
            {
                MsgBox(e.CommandArgument.ToString() + " 重覆送單 \r\n 此單已簽核 或是 在簽核中", this.Page, this);
            }
            else
            {
                //檢查並送出UOF               
                ADD_PURTCPURTD_TB_WKF_EXTERNAL_TASK("PUR50.採購變更單-大潁", e.CommandArgument.ToString());
            }

        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    public DataTable CHECK_TB_WKF_TASK(string TE001TE002TE003)
    {
        string TE001 = TE001TE002TE003.Substring(0, 4);
        string TE002 = TE001TE002TE003.Substring(4, 11);
        string TE003 = TE001TE002TE003.Substring(11, 4);

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                           WITH TEMP AS (
                            SELECT 
                                [FORM_NAME],
                                [DOC_NBR],
	                            [CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE001""]/@fieldValue)[1]', 'NVARCHAR(100)') AS TE001,
                                [CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE002""]/@fieldValue)[1]', 'NVARCHAR(100)') AS TE002,
	                            [CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE002""]/@fieldValue)[1]', 'NVARCHAR(100)') AS TE003,
                                TASK_ID,
                                TASK_STATUS,
                                TASK_RESULT
                                FROM[UOF].[dbo].TB_WKF_TASK
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM_VERSION] ON[TB_WKF_FORM_VERSION].FORM_VERSION_ID = TB_WKF_TASK.FORM_VERSION_ID
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM] ON[TB_WKF_FORM].FORM_ID = [TB_WKF_FORM_VERSION].FORM_ID
                                WHERE[FORM_NAME] = 'PUR50.採購變更單-大潁'
                                AND(TASK_STATUS IN('1') OR(TASK_STATUS IN('2') AND TASK_RESULT = '0'))


                            )
                            SELECT*
                            FROM TEMP
                            WHERE TE001 = '{0}' AND TE002 = '{1}' AND TE003='{2}'

                              ", TE001, TE002, TE003);




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

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
    public void ADD_PURTCPURTD_TB_WKF_EXTERNAL_TASK(string FORMNAME, string TE001TE002TE003)
    {
        string TE001 = TE001TE002TE003.Substring(0, 4);
        string TE002 = TE001TE002TE003.Substring(4, 11);
        string TE003 = TE001TE002TE003.Substring(15, 4);


        DataTable DT = SEARCHPURTEPURTF(TE001, TE002, TE003);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["TE037"].ToString());

        string account = DT.Rows[0]["TE037"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["MV002"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = "DY-" + DT.Rows[0]["TE001"].ToString().Trim() + DT.Rows[0]["TE002"].ToString().Trim() + DT.Rows[0]["TE003"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string PURTEID = SEARCHFORM_UOF_VERSION_ID("PUR50.採購變更單-大潁");

        if (!string.IsNullOrEmpty(PURTEID))
        {
            Form.SetAttribute("formVersionId", PURTEID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE004
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE006
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE006"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE005
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE005"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE005NAME
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE005NAME");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE005NAME"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE007
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE007");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE007"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE008
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE008"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE009
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE009");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE009"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE010
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE010");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE010"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE023
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE023");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE023"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE011
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE011");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE011"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE012
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE012");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE012"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE015
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE015");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE015"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE018
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE018");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE018"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE018NAME
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE018NAME");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE018NAME"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE019
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE019");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE019"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE020
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE020");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE020"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE022
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE022");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE022"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE024
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE024");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE024"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE027
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE027");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE027"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE037
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE037");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE037"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE037NAME
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE037NAME");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE037NAME"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE043
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE043");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE043"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE045
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE045");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE045"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TE046
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TE046");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TE046"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);







        //建立節點FieldItem
        //PURTF
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "PURTF");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點 DataGrid
        XmlElement DataGrid = xmlDoc.CreateElement("DataGrid");
        //DataGrid 加入至 TB 節點底下
        XmlNode PURTD = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='PURTF']");
        PURTD.AppendChild(DataGrid);


        foreach (DataRow od in DT.Rows)
        {
            // 新增 Row
            XmlElement Row = xmlDoc.CreateElement("Row");
            Row.SetAttribute("order", (rowscounts).ToString());

            //Row	TF004
            XmlElement Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF004");
            Cell.SetAttribute("fieldValue", od["TF004"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF005
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF005");
            Cell.SetAttribute("fieldValue", od["TF005"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF006
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF006");
            Cell.SetAttribute("fieldValue", od["TF006"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF007
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF007");
            Cell.SetAttribute("fieldValue", od["TF007"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF008
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF008");
            Cell.SetAttribute("fieldValue", od["TF008"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF009
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF009");
            Cell.SetAttribute("fieldValue", od["TF009"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF010
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF010");
            Cell.SetAttribute("fieldValue", od["TF010"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF011
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF011");
            Cell.SetAttribute("fieldValue", od["TF011"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF012
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF012");
            Cell.SetAttribute("fieldValue", od["TF012"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF013
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF013");
            Cell.SetAttribute("fieldValue", od["TF013"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF014
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF014");
            Cell.SetAttribute("fieldValue", od["TF014"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF015
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF015");
            Cell.SetAttribute("fieldValue", od["TF015"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF017
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF017");
            Cell.SetAttribute("fieldValue", od["TF017"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF018
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF018");
            Cell.SetAttribute("fieldValue", od["TF018"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF021
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF021");
            Cell.SetAttribute("fieldValue", od["TF021"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF022
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF022");
            Cell.SetAttribute("fieldValue", od["TF022"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TF030
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TF030");
            Cell.SetAttribute("fieldValue", od["TF030"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);


            rowscounts = rowscounts + 1;

            XmlNode DataGridS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='PURTF']/DataGrid");
            DataGridS.AppendChild(Row);

        }


        //用ADDTACK，直接啟動起單
        ADDTACK(Form);              

    }

    public DataTable SEARCHPURTEPURTF(string TE001, string TE002, string TE003)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        StringBuilder sbSql = new StringBuilder();
        StringBuilder sbSqlQuery = new StringBuilder();             

        try       
        {           

            sbSql.Clear();
            sbSqlQuery.Clear();

          

            sbSql.AppendFormat(@"  
                                   SELECT *
                                    ,USER_GUID,NAME
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'
                                    ,SUMLA011
                                    ,MA002 AS TE005NAME
                                    ,(CASE WHEN TE018='1' THEN '1.應稅內含'  WHEN TE018='2' THEN '2.應稅外加'  WHEN TE018='3' THEN '3.零稅率' WHEN TE018='4' THEN '4.免稅' WHEN TE018='9' THEN '9.不計稅' END) AS TE018NAME
                                    ,NAME AS TE037NAME
                                    FROM 
                                    (
                                    SELECT 
                                    [PURTE].[COMPANY]
                                    ,[PURTE].[CREATOR]
                                    ,[PURTE].[USR_GROUP]
                                    ,[PURTE].[CREATE_DATE]
                                    ,[PURTE].[MODIFIER]
                                    ,[PURTE].[MODI_DATE]
                                    ,[PURTE].[FLAG]
                                    ,[PURTE].[CREATE_TIME]
                                    ,[PURTE].[MODI_TIME]
                                    ,[PURTE].[TRANS_TYPE]
                                    ,[PURTE].[TRANS_NAME]
                                    ,[PURTE].[sync_date]
                                    ,[PURTE].[sync_time]
                                    ,[PURTE].[sync_mark]
                                    ,[PURTE].[sync_count]
                                    ,[PURTE].[DataUser]
                                    ,[PURTE].[DataGroup]
                                    ,[PURTE].[TE001]
                                    ,[PURTE].[TE002]
                                    ,[PURTE].[TE003]
                                    ,[PURTE].[TE004]
                                    ,[PURTE].[TE005]
                                    ,[PURTE].[TE006]
                                    ,[PURTE].[TE007]
                                    ,[PURTE].[TE008]
                                    ,[PURTE].[TE009]
                                    ,[PURTE].[TE010]
                                    ,[PURTE].[TE011]
                                    ,[PURTE].[TE012]
                                    ,[PURTE].[TE013]
                                    ,[PURTE].[TE014]
                                    ,[PURTE].[TE015]
                                    ,[PURTE].[TE016]
                                    ,[PURTE].[TE017]
                                    ,[PURTE].[TE018]
                                    ,[PURTE].[TE019]
                                    ,[PURTE].[TE020]
                                    ,[PURTE].[TE021]
                                    ,[PURTE].[TE022]
                                    ,[PURTE].[TE023]
                                    ,[PURTE].[TE024]
                                    ,[PURTE].[TE025]
                                    ,[PURTE].[TE026]
                                    ,[PURTE].[TE027]
                                    ,[PURTE].[TE028]
                                    ,[PURTE].[TE029]
                                    ,[PURTE].[TE030]
                                    ,[PURTE].[TE031]
                                    ,[PURTE].[TE032]
                                    ,[PURTE].[TE033]
                                    ,[PURTE].[TE034]
                                    ,[PURTE].[TE035]
                                    ,[PURTE].[TE036]
                                    ,[PURTE].[TE037]
                                    ,[PURTE].[TE038]
                                    ,[PURTE].[TE039]
                                    ,[PURTE].[TE040]
                                    ,[PURTE].[TE041]
                                    ,[PURTE].[TE042]
                                    ,[PURTE].[TE043]
                                    ,[PURTE].[TE045]
                                    ,[PURTE].[TE046]
                                    ,[PURTE].[TE047]
                                    ,[PURTE].[TE048]
                                    ,[PURTE].[TE103]
                                    ,[PURTE].[TE107]
                                    ,[PURTE].[TE108]
                                    ,[PURTE].[TE109]
                                    ,[PURTE].[TE110]
                                    ,[PURTE].[TE113]
                                    ,[PURTE].[TE114]
                                    ,[PURTE].[TE115]
                                    ,[PURTE].[TE118]
                                    ,[PURTE].[TE119]
                                    ,[PURTE].[TE120]
                                    ,[PURTE].[TE121]
                                    ,[PURTE].[TE122]
                                    ,[PURTE].[TE123]
                                    ,[PURTE].[TE124]
                                    ,[PURTE].[TE125]
                                    ,[PURTE].[TE134]
                                    ,[PURTE].[TE135]
                                    ,[PURTE].[TE136]
                                    ,[PURTE].[TE137]
                                    ,[PURTE].[TE138]
                                    ,[PURTE].[TE139]
                                    ,[PURTE].[TE140]
                                    ,[PURTE].[TE141]
                                    ,[PURTE].[TE142]
                                    ,[PURTE].[TE143]
                                    ,[PURTE].[TE144]
                                    ,[PURTE].[TE145]
                                    ,[PURTE].[TE146]
                                    ,[PURTE].[TE147]
                                    ,[PURTE].[TE148]
                                    ,[PURTE].[TE149]
                                    ,[PURTE].[TE150]
                                    ,[PURTE].[TE151]
                                    ,[PURTE].[TE152]
                                    ,[PURTE].[TE153]
                                    ,[PURTE].[TE154]
                                    ,[PURTE].[TE155]
                                    ,[PURTE].[TE156]
                                    ,[PURTE].[TE157]
                                    ,[PURTE].[TE158]
                                    ,[PURTE].[TE159]
                                    ,[PURTE].[TE160]
                                    ,[PURTE].[TE161]
                                    ,[PURTE].[TE162]
                                    ,[PURTE].[UDF01]  AS 'PURTFUDE01'
                                    ,[PURTE].[UDF02]  AS 'PURTFUDE02'
                                    ,[PURTE].[UDF03]  AS 'PURTFUDE03'
                                    ,[PURTE].[UDF04]  AS 'PURTFUDE04'
                                    ,[PURTE].[UDF05]  AS 'PURTFUDE05'
                                    ,[PURTE].[UDF06]  AS 'PURTFUDE06'
                                    ,[PURTE].[UDF07]  AS 'PURTFUDE07'
                                    ,[PURTE].[UDF08]  AS 'PURTFUDE08'
                                    ,[PURTE].[UDF09]  AS 'PURTFUDE09'
                                    ,[PURTE].[UDF10]  AS 'PURTFUDE10'
                                    ,[PURTF].[TF001]
                                    ,[PURTF].[TF002]
                                    ,[PURTF].[TF003]
                                    ,[PURTF].[TF004]
                                    ,[PURTF].[TF005]
                                    ,[PURTF].[TF006]
                                    ,[PURTF].[TF007]
                                    ,[PURTF].[TF008]
                                    ,[PURTF].[TF009]
                                    ,[PURTF].[TF010]
                                    ,[PURTF].[TF011]
                                    ,[PURTF].[TF012]
                                    ,[PURTF].[TF013]
                                    ,[PURTF].[TF014]
                                    ,[PURTF].[TF015]
                                    ,[PURTF].[TF016]
                                    ,[PURTF].[TF017]
                                    ,[PURTF].[TF018]
                                    ,[PURTF].[TF019]
                                    ,[PURTF].[TF020]
                                    ,[PURTF].[TF021]
                                    ,[PURTF].[TF022]
                                    ,[PURTF].[TF023]
                                    ,[PURTF].[TF024]
                                    ,[PURTF].[TF025]
                                    ,[PURTF].[TF026]
                                    ,[PURTF].[TF027]
                                    ,[PURTF].[TF028]
                                    ,[PURTF].[TF029]
                                    ,[PURTF].[TF030]
                                    ,[PURTF].[TF031]
                                    ,[PURTF].[TF032]
                                    ,[PURTF].[TF033]
                                    ,[PURTF].[TF034]
                                    ,[PURTF].[TF035]
                                    ,[PURTF].[TF036]
                                    ,[PURTF].[TF037]
                                    ,[PURTF].[TF038]
                                    ,[PURTF].[TF039]
                                    ,[PURTF].[TF040]
                                    ,[PURTF].[TF041]
                                    ,[PURTF].[TF104]
                                    ,[PURTF].[TF105]
                                    ,[PURTF].[TF106]
                                    ,[PURTF].[TF107]
                                    ,[PURTF].[TF108]
                                    ,[PURTF].[TF109]
                                    ,[PURTF].[TF110]
                                    ,[PURTF].[TF111]
                                    ,[PURTF].[TF112]
                                    ,[PURTF].[TF113]
                                    ,[PURTF].[TF114]
                                    ,[PURTF].[TF118]
                                    ,[PURTF].[TF119]
                                    ,[PURTF].[TF120]
                                    ,[PURTF].[TF121]
                                    ,[PURTF].[TF122]
                                    ,[PURTF].[TF123]
                                    ,[PURTF].[TF124]
                                    ,[PURTF].[TF125]
                                    ,[PURTF].[TF126]
                                    ,[PURTF].[TF127]
                                    ,[PURTF].[TF128]
                                    ,[PURTF].[TF129]
                                    ,[PURTF].[TF130]
                                    ,[PURTF].[TF131]
                                    ,[PURTF].[TF132]
                                    ,[PURTF].[TF133]
                                    ,[PURTF].[TF134]
                                    ,[PURTF].[TF135]
                                    ,[PURTF].[TF136]
                                    ,[PURTF].[TF137]
                                    ,[PURTF].[TF138]
                                    ,[PURTF].[TF139]
                                    ,[PURTF].[TF140]
                                    ,[PURTF].[TF141]
                                    ,[PURTF].[TF142]
                                    ,[PURTF].[TF143]
                                    ,[PURTF].[TF144]
                                    ,[PURTF].[TF145]
                                    ,[PURTF].[TF146]
                                    ,[PURTF].[TF147]
                                    ,[PURTF].[TF148]
                                    ,[PURTF].[TF149]
                                    ,[PURTF].[TF150]
                                    ,[PURTF].[TF151]
                                    ,[PURTF].[TF152]
                                    ,[PURTF].[TF153]
                                    ,[PURTF].[TF154]
                                    ,[PURTF].[TF155]
                                    ,[PURTF].[TF156]
                                    ,[PURTF].[TF157]
                                    ,[PURTF].[TF158]
                                    ,[PURTF].[TF159]
                                    ,[PURTF].[TF160]
                                    ,[PURTF].[TF161]
                                    ,[PURTF].[TF162]
                                    ,[PURTF].[TF163]
                                    ,[PURTF].[TF164]
                                    ,[PURTF].[TF165]
                                    ,[PURTF].[TF166]
                                    ,[PURTF].[TF167]
                                    ,[PURTF].[TF168]
                                    ,[PURTF].[TF169]
                                    ,[PURTF].[TF170]
                                    ,[PURTF].[TF171]
                                    ,[PURTF].[TF172]
                                    ,[PURTF].[TF173]
                                    ,[PURTF].[UDF01] AS 'PURTFUDF01'
                                    ,[PURTF].[UDF02] AS 'PURTFUDF02'
                                    ,[PURTF].[UDF03] AS 'PURTFUDF03'
                                    ,[PURTF].[UDF04] AS 'PURTFUDF04'
                                    ,[PURTF].[UDF05] AS 'PURTFUDF05'
                                    ,[PURTF].[UDF06] AS 'PURTFUDF06'
                                    ,[PURTF].[UDF07] AS 'PURTFUDF07'
                                    ,[PURTF].[UDF08] AS 'PURTFUDF08'
                                    ,[PURTF].[UDF09] AS 'PURTFUDF09'
                                    ,[PURTF].[UDF10] AS 'PURTFUDF10'
                                    ,[TB_EB_USER].USER_GUID,NAME
                                    ,(SELECT TOP 1 MV002 FROM [DY].dbo.CMSMV WHERE MV001=TE037) AS 'MV002'
                                    ,(SELECT TOP 1 MA002 FROM [DY].dbo.PURMA WHERE MA001=TE005) AS 'MA002'
                                    ,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [DY].dbo.INVLA WITH(NOLOCK) WHERE LA001=TF005 AND LA009 IN ('20004','20006','20008','20019','20020')) AS SUMLA011
                                    FROM [DY].dbo.PURTF,[DY].dbo.PURTE
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TE037 COLLATE Chinese_Taiwan_Stroke_BIN
                                    WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                                    AND TE001='{0}' AND TE002='{1}' AND TE003='{2}'
                                    ) AS TEMP
                              
                                    ", TE001, TE002,TE003);

            DataTable dt = new DataTable();
            dt.Load(m_db.ExecuteReader(sbSql.ToString()));

            if (dt.Rows.Count >= 1)
            {
                return dt;

            }
            else
            {
                return null;
            }

        }
        catch
        {
            return null;
        }
        finally
        {
          
        }
    }

    public DataTable SEARCHUOFDEP(string ACCOUNT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        StringBuilder sbSql = new StringBuilder();
        StringBuilder sbSqlQuery = new StringBuilder();

        try
        {           
            sbSql.Clear();
            sbSqlQuery.Clear();

            sbSql.AppendFormat(@"  
                                    SELECT 
                                    [GROUP_NAME] AS 'DEPNAME'
                                    ,[TB_EB_EMPL_DEP].[GROUP_ID]+','+[GROUP_NAME]+',False' AS 'DEPNO'
                                    ,[TB_EB_USER].[USER_GUID]
                                    ,[ACCOUNT]
                                    ,[NAME]
                                    ,[TB_EB_EMPL_DEP].[GROUP_ID]
                                    ,[TITLE_ID]     
                                    ,[GROUP_NAME]
                                    ,[GROUP_CODE]
                                    ,[TB_EB_EMPL_DEP].ORDERS
                                    FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER],[192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP],[192.168.1.223].[UOF].[dbo].[TB_EB_GROUP]
                                    WHERE [TB_EB_USER].[USER_GUID]=[TB_EB_EMPL_DEP].[USER_GUID]
                                    AND [TB_EB_EMPL_DEP].[GROUP_ID]=[TB_EB_GROUP].[GROUP_ID]
                                    AND ISNULL([TB_EB_GROUP].[GROUP_CODE],'')<>''
                                    AND [ACCOUNT]='{0}'
                                    ORDER BY [TB_EB_EMPL_DEP].ORDERS

                                    ", ACCOUNT);



            DataTable dt = new DataTable();
            dt.Load(m_db.ExecuteReader(sbSql.ToString()));

            if (dt.Rows.Count >= 1)
            {
                return dt;

            }
            else
            {
                return null;
            }

        }
        catch
        {
            return null;
        }
        finally
        {
           
        }
    }
    public string SEARCHFORM_UOF_VERSION_ID(string FORM_NAME)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();

            cmdTxt.AppendFormat(@" 
                                 SELECT TOP 1 RTRIM(LTRIM(TB_WKF_FORM_VERSION.FORM_VERSION_ID)) FORM_VERSION_ID,TB_WKF_FORM_VERSION.FORM_ID,TB_WKF_FORM_VERSION.VERSION,TB_WKF_FORM_VERSION.ISSUE_CTL
                                    ,TB_WKF_FORM.FORM_NAME
                                    FROM [UOF].dbo.TB_WKF_FORM_VERSION,[UOF].dbo.TB_WKF_FORM
                                    WHERE 1=1
                                    AND TB_WKF_FORM_VERSION.FORM_ID=TB_WKF_FORM.FORM_ID
                                    AND TB_WKF_FORM_VERSION.ISSUE_CTL=1
                                    AND FORM_NAME='{0}'
                                    ORDER BY TB_WKF_FORM_VERSION.FORM_ID,TB_WKF_FORM_VERSION.VERSION DESC

                                   ", FORM_NAME);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt.Rows.Count >= 1)
            {
                return dt.Rows[0]["FORM_VERSION_ID"].ToString();
            }
            else
            {
                return "";
            }

        }
        catch
        {
            return "";
        }
        finally
        {

        }
    }


    public void ADDTACK(XmlElement Form)
    {
        Ede.Uof.WKF.Utility.TaskUtilityUCO taskUCO = new Ede.Uof.WKF.Utility.TaskUtilityUCO();

        string result = taskUCO.WebService_CreateTask(Form.OuterXml);

        XElement resultXE = XElement.Parse(result);

        string status = "";
        string formNBR = "";
        string error = "";

        string NEWTASK_ID = "";

        if (resultXE.Element("Status").Value == "1")
        {
            status = "送單成功!";
            formNBR = resultXE.Element("FormNumber").Value;

            NEWTASK_ID = formNBR;

            Logger.Write("起單", status + formNBR);
            MsgBox("送單成功 \r\n" + TC001 + TC002 + " > " + formNBR, this.Page, this);

        }
        else
        {
            status = "送單失敗!";
            error = resultXE.Element("Exception").Element("Message").Value;

            Logger.Write("起單", status + error + "\r\n" + Form.OuterXml);

            MsgBox("失敗了，無法送單!!!!    " + error + "\r\n" + Form.OuterXml, this.Page, this);

            throw new Exception(status + error + "\r\n" + Form.OuterXml);

        }
    }
    public void MsgBox(String ex, Page pg, Object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);

        //string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        //Type cstype = obj.GetType();
        //ClientScriptManager cs = pg.ClientScript;
        //cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }

    #endregion

    #region BUTTON
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    #endregion
}