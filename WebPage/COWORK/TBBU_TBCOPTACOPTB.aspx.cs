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

public partial class CDS_WebPage_COWORK_TBBU_TBCOPTACOPTB : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    string DBNAME = "UOF";
    //string DBNAME = "UOFTEST";

    string TA001 = "";
    string TA002 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            SETDATES();
            BindGrid();
        }
    }

    #region FUNCTION
    public void SETDATES()
    {
        TextBox1.Text = DateTime.Now.ToString("yyyy");
    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //核單
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" 
                                AND TA003 LIKE '{0}%'
                                ", TextBox1.Text);
        }
        else
        {

        }

        cmdTxt.AppendFormat(@" 
                                SELECT TA006,TA001,TA002,TB003,TB004,TB005,TB009,TB010,TA001+TA002 AS TA001TA002
                                FROM [TK].dbo.COPTA,[TK].dbo.COPTB
                                WHERE TA001=TB001 AND TA002=TB002
                                AND TA019 IN ('N')
                                {0}
                                ORDER BY TA001,TA002  

                                ", QUERYS.ToString());




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
            //Grid1_Button1
            //Get the button that raised the event
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid1_Button1");
            //Get the row that contains this button
            GridViewRow gvr1 = (GridViewRow)Grid1_Button1.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue1 = Grid1_Button1.CommandArgument;
            DataRowView row1 = (DataRowView)e.Row.DataItem;
            Button lbtnName1 = (Button)e.Row.FindControl("Grid1_Button1");
            ExpandoObject param1 = new { ID = Cellvalue1 }.ToExpando();

        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid1_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "", this.Page, this);

            TA001 = e.CommandArgument.ToString().Substring(0, 4);
            TA002 = e.CommandArgument.ToString().Substring(4, 11);

            ADDTB_WKF_EXTERNAL_TASK_COPTACOPTB(TA001, TA002);
        }

    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();        
    }
    public void MsgBox(String ex, Page pg, Object obj)
    {
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }

    public void ADDTB_WKF_EXTERNAL_TASK_COPTACOPTB(string TA001, string TB002)
    {
        DataTable DT = SEARCHCOPTACOPTBCOPTC(TA001, TA002);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["TA005"].ToString());

        string account = DT.Rows[0]["TA005"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["MV002"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["TA001"].ToString().Trim() + DT.Rows[0]["TA002"].ToString().Trim();

        int rowscounts = 0;

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string COPTAID = SEARCHFORM_VERSION_ID("COP01.報價單");

        if (!string.IsNullOrEmpty(COPTAID))
        {
            Form.SetAttribute("formVersionId", COPTAID);
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
        //TA001	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA002	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA003	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA004	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA006	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA006"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA005	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA005");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA005"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA022
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA022");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA022"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA026	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA026");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA026"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA011NAME	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA011NAME");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA011"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA007	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA007");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA007"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //TA008	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TA008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TA008"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //MA010
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "MA010");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["MA010"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);


        //建立節點FieldItem
        //DETAILS 
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "DETAILS");
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
        XmlNode TB = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='DETAILS']");
        TB.AppendChild(DataGrid);


        foreach (DataRow od in DT.Rows)
        {
            // 新增 Row
            XmlElement Row = xmlDoc.CreateElement("Row");
            Row.SetAttribute("order", (rowscounts).ToString());

            //Row	TB003
            XmlElement Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TB003");
            Cell.SetAttribute("fieldValue", od["TB003"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TB004
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TB004");
            Cell.SetAttribute("fieldValue", od["TB004"].ToString());
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

            //Row	TB007
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TB007");
            Cell.SetAttribute("fieldValue", od["TB007"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TB008
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TB008");
            Cell.SetAttribute("fieldValue", od["TB008"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TB009
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TB009");
            Cell.SetAttribute("fieldValue", od["TB009"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TB010
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TB010");
            Cell.SetAttribute("fieldValue", od["TB010"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TB016
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TB016");
            Cell.SetAttribute("fieldValue", od["TB016"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TK004
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TK004");
            Cell.SetAttribute("fieldValue", od["TK004"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TK006
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TK006");
            Cell.SetAttribute("fieldValue", od["TK006"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);


            rowscounts = rowscounts + 1;

            XmlNode DataGridS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='DETAILS']/DataGrid");
            DataGridS.AppendChild(Row);

        }


        //用ADDTACK，直接啟動起單
        ADDTACK(Form);


    }

    public string SEARCHFORM_VERSION_ID(string FORM_NAME)
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
    public DataTable SEARCHCOPTACOPTBCOPTC(string TA001, string TA002)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
                                   SELECT TEMP.*
                                    ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
                                    ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'

                                    FROM 
                                    (
                                    SELECT COPTA.CREATOR,TA001,TA002,TA003,TA004,TA005,TA006,TA007,TA008,TA009,TA010,TA011,TA026
                                    ,(CASE WHEN TA022='1' THEN '內含'  WHEN TA022='2' THEN '外加'  WHEN TA022='3' THEN '零稅率'   WHEN TA022='4' THEN '免稅' WHEN TA022='9' THEN '不計稅' ELSE '空白' END ) TA022
                                    ,TB003,TB004,TB005,TB006,TB007,TB008,TB009,TB010,TB011,TB016
                                    ,ISNULL(TK004,0) AS TK004,ISNULL(TK006,0) AS TK006
                                    ,[TB_EB_USER].USER_GUID,NAME
                                    ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TA005) AS 'MV002'
                                    ,MA010

                                    FROM [TK].dbo.COPTA
                                    LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TA005 COLLATE Chinese_Taiwan_Stroke_BIN
                                    INNER JOIN [TK].dbo.COPTB ON TA001=TB001 AND TA002=TB002
                                    LEFT JOIN [TK].dbo.COPTK ON  TK001=TB001 AND TK002=TB002 AND TK003=TB003
                                    LEFT JOIN [TK].dbo.COPMA ON MA001=TA004

                                    WHERE 
                                    TA001='{0}' AND TA002='{1}'
                                    ) AS TEMP
                              
                                    ", TA001, TA002);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
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
        { }
    }

    public DataTable SEARCHUOFDEP(string ACCOUNT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"  
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
                                    FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER],[192.168.1.223].[UOF].[dbo].[TB_EB_EMPL_DEP],[192.168.1.223].[UOF].[dbo].[TB_EB_GROUP]
                                    WHERE [TB_EB_USER].[USER_GUID]=[TB_EB_EMPL_DEP].[USER_GUID]
                                    AND [TB_EB_EMPL_DEP].[GROUP_ID]=[TB_EB_GROUP].[GROUP_ID]
                                    AND ISNULL([TB_EB_GROUP].[GROUP_CODE],'')<>''
                                    AND [ACCOUNT]='{0}'
                              
                                    ", ACCOUNT);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
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
        { }
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
            MsgBox("送單成功 \r\n" + TA001 + TA002 + " > " + formNBR, this.Page, this);

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

    #endregion

    #region BUTTON
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();
      

    }
    #endregion
}