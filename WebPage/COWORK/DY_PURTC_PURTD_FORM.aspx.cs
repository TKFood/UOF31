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

public partial class CDS_WebPage_COWORK_DY_PURTC_PURTD_FORM : Ede.Uof.Utility.Page.BasePage
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
            QUERYS.AppendFormat(@" AND TC002 LIKE '{0}%'", TextBox1.Text.Trim() + TextBox2.Text.Trim());

        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TC014='N'");
            }
            else if (DropDownList1.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TC014='Y'");
            }
        }

        cmdTxt.AppendFormat(@" 
                            SELECT 
                            REPLACE(TC001+TC002,' ','') AS TC001TC002,
                            TC001,
                            TC002,
                            TC003,
                            TC004,
                            MA002,
                            STUFF((
                                    SELECT ',' + TD005+' ,數量'+CONVERT(NVARCHAR,TD008)+' ,到貨日'+TD012
                                    FROM [DY].dbo.PURTD
                                    WHERE TD001=TC001 AND TD002=TC002
                                    FOR XML PATH(''), TYPE
                                ).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS DETAILS

                            FROM [DY].dbo.PURTC,[DY].dbo.PURMA
                            WHERE TC004=MA001
                            {0}
                            {1}
                            ORDER BY TC001,TC002
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
                ADD_PURTCPURTD_TB_WKF_EXTERNAL_TASK("PUR40.採購單-大潁",e.CommandArgument.ToString());
            }

        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    public DataTable CHECK_TB_WKF_TASK(string TC001TC002)
    {
        string TC001 = TC001TC002.Substring(0, 4);
        string TC002 = TC001TC002.Substring(4, 11);

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            WITH TEMP AS (
                            SELECT 
                                [FORM_NAME],
                                [DOC_NBR],
	                            [CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""TC001""]/@fieldValue)[1]', 'NVARCHAR(100)') AS TC001,
                                [CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""TC002""]/@fieldValue)[1]', 'NVARCHAR(100)') AS TC002,
                                TASK_ID,
                                TASK_STATUS,
                                TASK_RESULT
                                FROM[UOF].[dbo].TB_WKF_TASK
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM_VERSION] ON[TB_WKF_FORM_VERSION].FORM_VERSION_ID = TB_WKF_TASK.FORM_VERSION_ID
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM] ON[TB_WKF_FORM].FORM_ID = [TB_WKF_FORM_VERSION].FORM_ID
                                WHERE[FORM_NAME] = 'PUR40.採購單-大潁'
                                AND(TASK_STATUS IN('1') OR(TASK_STATUS IN('2') AND TASK_RESULT = '0'))


                            )
                            SELECT*
                            FROM TEMP
                            WHERE TC001 = '{0}' AND TC002 = '{1}'

                              ", TC001, TC002);




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
    public void ADD_PURTCPURTD_TB_WKF_EXTERNAL_TASK(string FORMNAME, string TC001TC002)
    {
        string TC001 = TC001TC002.Substring(0, 4);
        string TC002 = TC001TC002.Substring(4, 11);

        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlComm = new SqlCommand();
        StringBuilder sbSql = new StringBuilder();
        StringBuilder sbSqlQuery = new StringBuilder();

        //找出ERP的單據資料
        DataTable DT = SEARCHPURTCPURTD(TC001, TC002);
        //用建單人找出建單人+部門的UOF資訊
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["TC011"].ToString());



        string account = DT.Rows[0]["TC011"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["MV002"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = "DY-" + DT.Rows[0]["TC001"].ToString().Trim() + DT.Rows[0]["TC002"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string PURTCID = SEARCHFORM_UOF_VERSION_ID("PUR40.採購單-大潁");

        if (!string.IsNullOrEmpty(PURTCID))
        {
            Form.SetAttribute("formVersionId", PURTCID);
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
        //TC001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC004NAME	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC004NAME");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC004NAME"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC010	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC010");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC010"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC005"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC006"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC027	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC027");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC027"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC008"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC028	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC028");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC028"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC009	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC009");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC009"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC018	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC018");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC018"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC018NAME	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC018NAME");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC018NAME"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //	TC011
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC011");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC011"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC011NAME	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC011NAME");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC011NAME"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC037	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC037");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC037"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC038	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC038");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC038"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC021	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC021");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC021"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //PURTD
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "PURTD");
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
        XmlNode PURTD = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='PURTD']");
        PURTD.AppendChild(DataGrid);


        foreach (DataRow od in DT.Rows)
        {
            // 新增 Row
            XmlElement Row = xmlDoc.CreateElement("Row");
            Row.SetAttribute("order", (rowscounts).ToString());

            //Row	TD003
            XmlElement Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD003");
            Cell.SetAttribute("fieldValue", od["TD003"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TB005
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TB005");
            Cell.SetAttribute("fieldValue", od["TB005"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD004
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD004");
            Cell.SetAttribute("fieldValue", od["TD004"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD005
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD005");
            Cell.SetAttribute("fieldValue", od["TD005"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD006
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD006");
            Cell.SetAttribute("fieldValue", od["TD006"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD007
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD007");
            Cell.SetAttribute("fieldValue", od["TD007"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD008
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD008");
            Cell.SetAttribute("fieldValue", od["TD008"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD009
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD009");
            Cell.SetAttribute("fieldValue", od["TD009"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD010
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD010");
            Cell.SetAttribute("fieldValue", od["TD010"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD011
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD011");
            Cell.SetAttribute("fieldValue", od["TD011"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD012
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD012");
            Cell.SetAttribute("fieldValue", od["TD012"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD015
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD015");
            Cell.SetAttribute("fieldValue", od["TD015"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD019
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD019");
            Cell.SetAttribute("fieldValue", od["TD019"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD026
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD026");
            Cell.SetAttribute("fieldValue", od["TD026"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD027
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD027");
            Cell.SetAttribute("fieldValue", od["TD027"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD028
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD028");
            Cell.SetAttribute("fieldValue", od["TD028"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD014
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD014");
            Cell.SetAttribute("fieldValue", od["TD014"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            rowscounts = rowscounts + 1;

            XmlNode DataGridS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='PURTD']/DataGrid");
            DataGridS.AppendChild(Row);

        }

       
        //用ADDTACK，直接啟動起單
        ADDTACK(Form);              

    }

    public DataTable SEARCHPURTCPURTD(string TC001, string TC002)
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
                                    ,MA002 AS TC004NAME
                                    ,(CASE WHEN TC018='1' THEN '1.應稅內含'  WHEN TC018='2' THEN '2.應稅外加'  WHEN TC018='3' THEN '3.零稅率' WHEN TC018='4' THEN '4.免稅' WHEN TC018='9' THEN '9.不計稅' END) AS TC018NAME
                                    ,NAME AS TC011NAME
                                    FROM 
                                    (
                                        SELECT 
                                        [PURTC].[COMPANY]
                                        ,[PURTC].[CREATOR]
                                        ,[PURTC].[USR_GROUP]
                                        ,[PURTC].[CREATE_DATE]
                                        ,[PURTC].[MODIFIER]
                                        ,[PURTC].[MODI_DATE]
                                        ,[PURTC].[FLAG]
                                        ,[PURTC].[CREATE_TIME]
                                        ,[PURTC].[MODI_TIME]
                                        ,[PURTC].[TRANS_TYPE]
                                        ,[PURTC].[TRANS_NAME]
                                        ,[PURTC].[sync_date]
                                        ,[PURTC].[sync_time]
                                        ,[PURTC].[sync_mark]
                                        ,[PURTC].[sync_count]
                                        ,[PURTC].[DataUser]
                                        ,[PURTC].[DataGroup]
                                        ,[PURTC].[TC001]
                                        ,[PURTC].[TC002]
                                        ,[PURTC].[TC003]
                                        ,[PURTC].[TC004]
                                        ,[PURTC].[TC005]
                                        ,[PURTC].[TC006]
                                        ,[PURTC].[TC007]
                                        ,[PURTC].[TC008]
                                        ,[PURTC].[TC009]
                                        ,[PURTC].[TC010]
                                        ,[PURTC].[TC011]
                                        ,[PURTC].[TC012]
                                        ,[PURTC].[TC013]
                                        ,[PURTC].[TC014]
                                        ,[PURTC].[TC015]
                                        ,[PURTC].[TC016]
                                        ,[PURTC].[TC017]
                                        ,[PURTC].[TC018]
                                        ,[PURTC].[TC019]
                                        ,[PURTC].[TC020]
                                        ,[PURTC].[TC021]
                                        ,[PURTC].[TC022]
                                        ,[PURTC].[TC023]
                                        ,[PURTC].[TC024]
                                        ,[PURTC].[TC025]
                                        ,[PURTC].[TC026]
                                        ,[PURTC].[TC027]
                                        ,[PURTC].[TC028]
                                        ,[PURTC].[TC029]
                                        ,[PURTC].[TC030]
                                        ,[PURTC].[TC031]
                                        ,[PURTC].[TC032]
                                        ,[PURTC].[TC033]
                                        ,[PURTC].[TC034]
                                        ,[PURTC].[TC035]
                                        ,[PURTC].[TC036]
                                        ,[PURTC].[TC037]
                                        ,[PURTC].[TC038]
                                        ,[PURTC].[TC039]
                                        ,[PURTC].[TC040]
                                        ,[PURTC].[TC041]
                                        ,[PURTC].[TC042]
                                        ,[PURTC].[TC043]
                                        ,[PURTC].[TC044]
                                        ,[PURTC].[TC045]
                                        ,[PURTC].[TC046]
                                        ,[PURTC].[TC047]
                                        ,[PURTC].[TC048]
                                        ,[PURTC].[TC049]
                                        ,[PURTC].[TC050]
                                        ,[PURTC].[TC051]
                                        ,[PURTC].[TC052]
                                        ,[PURTC].[TC053]
                                        ,[PURTC].[TC054]
                                        ,[PURTC].[TC055]
                                        ,[PURTC].[TC056]
                                        ,[PURTC].[TC057]
                                        ,[PURTC].[TC058]
                                        ,[PURTC].[TC059]
                                        ,[PURTC].[TC060]
                                        ,[PURTC].[TC061]
                                        ,[PURTC].[TC062]
                                        ,[PURTC].[TC063]
                                        ,[PURTC].[TC064]
                                        ,[PURTC].[TC065]
                                        ,[PURTC].[TC066]
                                        ,[PURTC].[TC067]
                                        ,[PURTC].[TC068]
                                        ,[PURTC].[TC069]
                                        ,[PURTC].[TC070]
                                        ,[PURTC].[TC071]
                                        ,[PURTC].[TC072]
                                        ,[PURTC].[TC073]
                                        ,[PURTC].[TC074]
                                        ,[PURTC].[TC075]
                                        ,[PURTC].[TC076]
                                        ,[PURTC].[TC077]
                                        ,[PURTC].[TC078]
                                        ,[PURTC].[TC079]
                                        ,[PURTC].[TC080]
                                        ,[PURTC].[UDF01] AS PURTCUDF01
                                        ,[PURTC].[UDF02] AS PURTCUDF02
                                        ,[PURTC].[UDF03] AS PURTCUDF03
                                        ,[PURTC].[UDF04] AS PURTCUDF04
                                        ,[PURTC].[UDF05] AS PURTCUDF05
                                        ,[PURTC].[UDF06] AS PURTCUDF06
                                        ,[PURTC].[UDF07] AS PURTCUDF07
                                        ,[PURTC].[UDF08] AS PURTCUDF08
                                        ,[PURTC].[UDF09] AS PURTCUDF09
                                        ,[PURTC].[UDF10] AS PURTCUDF10
                                        ,[PURTD].[TD001]
                                        ,[PURTD].[TD002]
                                        ,[PURTD].[TD003]
                                        ,[PURTD].[TD004]
                                        ,[PURTD].[TD005]
                                        ,[PURTD].[TD006]
                                        ,[PURTD].[TD007]
                                        ,[PURTD].[TD008]
                                        ,[PURTD].[TD009]
                                        ,[PURTD].[TD010]
                                        ,[PURTD].[TD011]
                                        ,[PURTD].[TD012]
                                        ,[PURTD].[TD013]
                                        ,[PURTD].[TD014]
                                        ,[PURTD].[TD015]
                                        ,[PURTD].[TD016]
                                        ,[PURTD].[TD017]
                                        ,[PURTD].[TD018]
                                        ,[PURTD].[TD019]
                                        ,[PURTD].[TD020]
                                        ,[PURTD].[TD021]
                                        ,[PURTD].[TD022]
                                        ,[PURTD].[TD023]
                                        ,[PURTD].[TD024]
                                        ,[PURTD].[TD025]
                                        ,[PURTD].[TD026]
                                        ,[PURTD].[TD027]
                                        ,[PURTD].[TD028]
                                        ,[PURTD].[TD029]
                                        ,[PURTD].[TD030]
                                        ,[PURTD].[TD031]
                                        ,[PURTD].[TD032]
                                        ,[PURTD].[TD033]
                                        ,[PURTD].[TD034]
                                        ,[PURTD].[TD035]
                                        ,[PURTD].[TD036]
                                        ,[PURTD].[TD037]
                                        ,[PURTD].[TD038]
                                        ,[PURTD].[TD039]
                                        ,[PURTD].[TD040]
                                        ,[PURTD].[TD041]
                                        ,[PURTD].[TD042]
                                        ,[PURTD].[TD043]
                                        ,[PURTD].[TD044]
                                        ,[PURTD].[TD045]
                                        ,[PURTD].[TD046]
                                        ,[PURTD].[TD047]
                                        ,[PURTD].[TD048]
                                        ,[PURTD].[TD049]
                                        ,[PURTD].[TD050]
                                        ,[PURTD].[TD051]
                                        ,[PURTD].[TD052]
                                        ,[PURTD].[TD053]
                                        ,[PURTD].[TD054]
                                        ,[PURTD].[TD055]
                                        ,[PURTD].[TD056]
                                        ,[PURTD].[TD057]
                                        ,[PURTD].[TD058]
                                        ,[PURTD].[TD059]
                                        ,[PURTD].[TD060]
                                        ,[PURTD].[TD061]
                                        ,[PURTD].[TD062]
                                        ,[PURTD].[TD063]
                                        ,[PURTD].[TD064]
                                        ,[PURTD].[TD065]
                                        ,[PURTD].[TD066]
                                        ,[PURTD].[TD067]
                                        ,[PURTD].[TD068]
                                        ,[PURTD].[TD069]
                                        ,[PURTD].[TD070]
                                        ,[PURTD].[TD071]
                                        ,[PURTD].[TD072]
                                        ,[PURTD].[TD073]
                                        ,[PURTD].[TD074]
                                        ,[PURTD].[TD075]
                                        ,[PURTD].[TD076]
                                        ,[PURTD].[TD077]
                                        ,[PURTD].[TD078]
                                        ,[PURTD].[TD079]
                                        ,[PURTD].[TD080]
                                        ,[PURTD].[TD081]
                                        ,[PURTD].[TD082]
                                        ,[PURTD].[TD083]
                                        ,[PURTD].[TD084]
                                        ,[PURTD].[TD085]
                                        ,[PURTD].[TD086]
                                        ,[PURTD].[TD087]
                                        ,[PURTD].[TD088]
                                        ,[PURTD].[TD089]
                                        ,[PURTD].[TD090]
                                        ,[PURTD].[TD091]
                                        ,[PURTD].[TD092]
                                        ,[PURTD].[TD093]
                                        ,[PURTD].[TD094]
                                        ,[PURTD].[TD095]
                                        ,[PURTD].[UDF01]  AS PURTDUDF01
                                        ,[PURTD].[UDF02]  AS PURTDUDF02
                                        ,[PURTD].[UDF03]  AS PURTDUDF03
                                        ,[PURTD].[UDF04]  AS PURTDUDF04
                                        ,[PURTD].[UDF05]  AS PURTDUDF05
                                        ,[PURTD].[UDF06]  AS PURTDUDF06
                                        ,[PURTD].[UDF07]  AS PURTDUDF07
                                        ,[PURTD].[UDF08]  AS PURTDUDF08
                                        ,[PURTD].[UDF09]  AS PURTDUDF09
                                        ,[PURTD].[UDF10]  AS PURTDUDF10
                                        ,[TB_EB_USER].USER_GUID,NAME
                                        ,(SELECT TOP 1 MV002 FROM [DY].dbo.CMSMV WHERE MV001=TC011) AS 'MV002'
                                        ,(SELECT TOP 1 MA002 FROM [DY].dbo.PURMA WHERE MA001=TC004) AS 'MA002'
                                        ,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [DY].dbo.INVLA WITH(NOLOCK) WHERE LA001=TD004 AND LA009 IN ('20004','20006','20008','20019','20020')) AS SUMLA011
                                        ,(SELECT TOP 1 CONVERT(NVARCHAR,TB005)+',需求日:'+CONVERT(NVARCHAR,TB011)+',數量:'+CONVERT(NVARCHAR,TB009)+' '+CONVERT(NVARCHAR,TB007) FROM  [DY].dbo.PURTB WHERE TB001=[PURTD].TD026 AND TB002=[PURTD].TD027 AND TB003=[PURTD].TD028) AS TB005
                                        FROM [DY].dbo.PURTD,[DY].dbo.PURTC
                                        LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TC011 COLLATE Chinese_Taiwan_Stroke_BIN
                                        WHERE TC001=TD001 AND TC002=TD002
                                        AND TC001='{0}' AND TC002='{1}'
                                    ) AS TEMP
                              
                                    ", TC001, TC002);

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